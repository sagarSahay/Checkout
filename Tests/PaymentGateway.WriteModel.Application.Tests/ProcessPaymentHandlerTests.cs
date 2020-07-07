namespace PaymentGateway.WriteModel.Application.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using AcquiringBankServices;
    using Commands;
    using Events.v1;
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
            private ICallBankApi callBankApi { get; }
            public ProcessPaymentHandler SUT { get; }

            public ProcessPayment Command { get; }

            public IPublishEndpoint PublishEndpoint { get; }
            public ConsumeContext<ProcessPayment> ConsumeContext { get; }

            public List<IEvent> FiredEvents { get; }


            public Arrangements(ICallBankApi callBankApi, 
                IServiceProvider serviceProvider,
                IPublishEndpoint publishEndpoint,
                ConsumeContext<ProcessPayment> consumeContext,
                List<IEvent> firedEvents)
            {
                this.callBankApi = callBankApi;
                ConsumeContext  = consumeContext;
                PublishEndpoint = publishEndpoint;
                FiredEvents = firedEvents;
                SUT = new ProcessPaymentHandler(serviceProvider);
            }
        }

        private class ArrangementsBuilder
        {
            private Mock<ICallBankApi> callBankApiMock;
            private ProcessPayment cmd;
            IServiceProvider serviceProvider;
            private Mock<ConsumeContext<ProcessPayment>> consumeContextMock;
            private Mock<IPublishEndpoint> publishEndpointMock;
            private List<IEvent> firedEvents = new List<IEvent>();

            public ArrangementsBuilder WithACommandWhichResultsInSuccess()
            {
                cmd = InitialiseAProcessPaymentCommand();
                callBankApiMock = new Mock<ICallBankApi>();

                callBankApiMock.Setup(x => x.CallBank(cmd)).Returns((Guid.NewGuid(), "SUCCESS"));
                
                SetupServiceProviderAndResolvePublishEndpoint(true);
                return this;
            }
            public ArrangementsBuilder WithACommandWhichResultsInFailure()
            {
                cmd = InitialiseAProcessPaymentCommand();
                callBankApiMock = new Mock<ICallBankApi>();

                callBankApiMock.Setup(x => x.CallBank(cmd)).Returns((Guid.NewGuid(), "UNSUCCESSFUL"));

                SetupServiceProviderAndResolvePublishEndpoint(false);
                return this;
            }
            
            public ArrangementsBuilder WithACommandWhichResultsInSystemError()
            {
                cmd = InitialiseAProcessPaymentCommand();
                callBankApiMock = new Mock<ICallBankApi>();

                callBankApiMock.Setup(x => x.CallBank(cmd)).Returns((Guid.NewGuid(), "System error: "));

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
                serviceCollection.AddScoped<ICallBankApi>(provider => callBankApiMock.Object);
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
               return new Arrangements(callBankApiMock.Object, 
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
        
        [Fact]
        public async Task ProcessPayment_WhenProcessingPaymentCausesSytemError_SendsPaymentErrorMessage()
        {
            // Arrange
            var arrangements = new ArrangementsBuilder()
                .WithACommandWhichResultsInSystemError()
                .Build();
            
            // Act
            await arrangements.SUT.Consume(arrangements.ConsumeContext);
            
            // Assert
            arrangements.FiredEvents.Count.Should().Be(1);

            var evt = arrangements.FiredEvents.FirstOrDefault();

            evt.Should().BeOfType<PaymentError>();
        }
    }
}
