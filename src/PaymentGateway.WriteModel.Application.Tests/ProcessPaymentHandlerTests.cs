using PaymentGateway.Events.v1;

namespace PaymentGateway.WriteModel.Application.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using AcquiringBankServices;
    using Commands;
    using FluentAssertions;
    using MassTransit;
    using Messages.CommandHandlers;
    using Microsoft.Extensions.DependencyInjection;
    using Moq;
    using PaymentGateway.Messages.Common;
    using Xunit;

    public class ProcessPaymentHandlerTests
    {
        private class Arrangements
        {
            private IAcquiringBank bank { get; }
            public ProcessPaymentHandler SUT { get; }

            public ProcessPayment Command { get; }

            public IPublishEndpoint PublishEndpoint { get; }
            public ConsumeContext<ProcessPayment> ConsumeContext { get; }

            public List<IEvent> FiredEvents { get; }


            public Arrangements(IAcquiringBank bank, 
                IServiceProvider serviceProvider,
                IPublishEndpoint publishEndpoint,
                ConsumeContext<ProcessPayment> consumeContext,
                List<IEvent> firedEvents)
            {
                this.bank = bank;
                ConsumeContext  = consumeContext;
                PublishEndpoint = publishEndpoint;
                FiredEvents = firedEvents;
                SUT = new ProcessPaymentHandler(serviceProvider);
            }
        }

        private class ArrangementsBuilder
        {
            private Mock<IAcquiringBank> acquiringBankMock;
            private Mock<IBankFactory> bankFactoryMock;
            private ProcessPayment cmd;
            IServiceProvider serviceProvider;
            private Mock<ConsumeContext<ProcessPayment>> consumeContextMock;
            private Mock<IPublishEndpoint> publishEndpointMock;
            private List<IEvent> firedEvents = new List<IEvent>();

            public ArrangementsBuilder WithACommandWhichResultsInSuccess()
            {
                cmd = InitialiseAProcessPaymentCommand();
                acquiringBankMock = new Mock<IAcquiringBank>();
                bankFactoryMock = new Mock<IBankFactory>();
                acquiringBankMock.Setup(x => x.ProcessPayment(cmd.CardNumber,
                cmd.Cvv,cmd.ExpiryDate,cmd.Amount,cmd.Currency, cmd.MerchantId)).Returns((Guid.NewGuid(), "SUCCESS"));

                bankFactoryMock.Setup(x => x.GetBank(cmd.MerchantId)).Returns(acquiringBankMock.Object);
                SetupServiceProviderAndResolvePublishEndpoint(true);
                return this;
            }
            public ArrangementsBuilder WithACommandWhichResultsInFailure()
            {
                cmd = InitialiseAProcessPaymentCommand();
                acquiringBankMock = new Mock<IAcquiringBank>();
                bankFactoryMock = new Mock<IBankFactory>();
                acquiringBankMock.Setup(x => x.ProcessPayment(cmd.CardNumber,
                    cmd.Cvv,cmd.ExpiryDate,cmd.Amount,cmd.Currency, cmd.MerchantId)).Returns((Guid.NewGuid(), "FAILURE"));

                bankFactoryMock.Setup(x => x.GetBank(cmd.MerchantId)).Returns(acquiringBankMock.Object);
                SetupServiceProviderAndResolvePublishEndpoint(false);
                return this;
            }

            private void SetupServiceProviderAndResolvePublishEndpoint(bool withSuccess)
            {
                publishEndpointMock  = new Mock<IPublishEndpoint>();
                if (withSuccess)
                {
                    publishEndpointMock.Setup(x => x.Publish(It.IsAny<IEvent>(), new CancellationToken()))
                        .Callback<IEvent, CancellationToken>((r,c) => firedEvents.Add(r));
                }
                else
                {
                    publishEndpointMock.Setup(x => x.Publish(It.IsAny<IEvent>(), new CancellationToken()))
                        .Callback<IEvent, CancellationToken>((r,c) => firedEvents.Add(r));
                }
                
                var serviceCollection = new ServiceCollection();
                serviceCollection.AddScoped<IPublishEndpoint>(provider => publishEndpointMock.Object);
                serviceCollection.AddScoped<IBankFactory>(provider => bankFactoryMock.Object);
                serviceProvider = serviceCollection.BuildServiceProvider();
            }
            private ProcessPayment InitialiseAProcessPaymentCommand()
            {
                var msg = new ProcessPayment()
                {
                    Amount = 12,
                    CardNumber = "1234-1234-1234-1234",
                    Currency = "£",
                    Cvv = "123",
                    ExpiryDate = "01/2021",
                    MerchantId = "merchant1",
                    OrderId = "Order-1",
                    PaymentId = Guid.NewGuid()
                };
                consumeContextMock = new Mock<ConsumeContext<ProcessPayment>>();
                consumeContextMock.Setup(x => x.Message).Returns(msg);
                return msg;
            }

            public Arrangements Build()
            {
               return new Arrangements(acquiringBankMock.Object, 
                   serviceProvider, 
                   publishEndpointMock.Object, 
                   consumeContextMock.Object,
                   firedEvents);
            }
        }

        [Fact]
        public async Task ProcessPayment_WhenPaymentIsSuccessful_SendsPaymentSuccessfulMessage()
        {
            // Arrange
            var arrangements = new ArrangementsBuilder()
                .WithACommandWhichResultsInSuccess()
                .Build();
            
            // Act
            await arrangements.SUT.Consume(arrangements.ConsumeContext);
            
            // Assert
            arrangements.FiredEvents.Count.Should().Be(1);

            var evt = arrangements.FiredEvents.FirstOrDefault();

            evt.Should().BeOfType<PaymentSuccessful>();
        }
        
        [Fact]
        public async Task ProcessPayment_WhenPaymentIsUnsuccessful_SendsPaymentUnsuccessfulMessage()
        {
            // Arrange
            var arrangements = new ArrangementsBuilder()
                .WithACommandWhichResultsInFailure()
                .Build();
            
            // Act
            await arrangements.SUT.Consume(arrangements.ConsumeContext);
            
            // Assert
            arrangements.FiredEvents.Count.Should().Be(1);

            var evt = arrangements.FiredEvents.FirstOrDefault();

            evt.Should().BeOfType<PaymentUnsuccessful>();
        }

    }
}
