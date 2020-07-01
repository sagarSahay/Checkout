using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using PaymentGateway.Commands;
using PaymentGateway.Events.v1;
using PaymentGateway.Messages.Common;
using PaymentGateway.WriteModel.Application.Messages.CommandHandlers;
using Xunit;

namespace PaymentGateway.WriteModel.Application.Tests
{
    public class ProcessPaymentHandlerTests
    {
        //private ProcessPaymentHandler SUT { get; set; }
        //private Mock<IAcquiringBank> bank { get; set; }
        //private Mock<IServiceProvider> serviceProvider;

        //[Fact]
        //public async Task ProcessPayment_WhenPaymentIsSuccessful_SendsPaymentSuccessfulMessage()
        //{
        //    // Arrange
        //    bank = new Mock<IAcquiringBank>();
        //    bank.Setup(x=> x.ProcessPayment(It.IsAny<Guid>(),))

        //}

        private class Arrangements
        {
            private IAcquiringBank bank { get; }
            public ProcessPaymentHandler SUT { get; }

            public ProcessPayment Command { get; }

            public IPublishEndpoint PublishEndpoint { get; }
            public ConsumeContext ConsumeContext { get; }

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
            private ProcessPayment cmd;
            private Mock<IServiceProvider> serviceProviderMock;
            private Mock<ConsumeContext<ProcessPayment>> consumeContextMock;
            private Mock<IPublishEndpoint> publishEndpointMock;
            private List<IEvent> firedEvents = new List<IEvent>();

            public ArrangementsBuilder WithACommandWhichResultsInSuccess()
            {
                cmd = InitialiseAProcessPaymentCommand();
                acquiringBankMock = new Mock<IAcquiringBank>();
                acquiringBankMock.Setup(x => x.ProcessPayment(cmd.CardNumber,
                cmd.Cvv,cmd.ExpiryDate,cmd.Amount,cmd.Currency)).Returns((Guid.NewGuid(), "SUCCESS"));

                SetupServiceProviderAndResolvePublishEndpoint(true);
                return this;
            }
            public ArrangementsBuilder WithACommandWhichResultsInFailure()
            {
                cmd = InitialiseAProcessPaymentCommand();
                acquiringBankMock.Setup(x => x.ProcessPayment(cmd.CardNumber,
                    cmd.Cvv, cmd.ExpiryDate, cmd.Amount, cmd.Currency)).Returns((Guid.NewGuid(), "FAILURE"));
                SetupServiceProviderAndResolvePublishEndpoint(false);
                return this;
            }

            private void SetupServiceProviderAndResolvePublishEndpoint(bool withSuccess)
            {
                publishEndpointMock  = new Mock<IPublishEndpoint>();
                if (withSuccess)
                {
                    publishEndpointMock.Setup(x => x.Publish(It.IsAny<IEvent>(), new CancellationToken()))
                        .Callback<PaymentSuccessful>(r => firedEvents.Add(r));
                }
                else
                {
                    publishEndpointMock.Setup(x => x.Publish(It.IsAny<IEvent>(), new CancellationToken()))
                        .Callback<PaymentUnsuccessful>(r => firedEvents.Add(r));
                }
                
                var serviceCollection = new ServiceCollection();
                serviceCollection.AddScoped<IPublishEndpoint>(provider => publishEndpointMock.Object);
                serviceCollection.AddScoped<AcquiringBankFactory>();
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

            //public Arrangements Build()
            //{
            //    serviceProvider.Setup(x=> x.GetService<IPublishEndpoint>()).Returns()
            //}


        }

    }
}
