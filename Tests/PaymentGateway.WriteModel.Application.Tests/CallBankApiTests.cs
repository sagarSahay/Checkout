namespace PaymentGateway.WriteModel.Application.Tests
{
    using System;
    using AcquiringBankServices;
    using Commands;
    using FluentAssertions;
    using Microsoft.Extensions.DependencyInjection;
    using Moq;
    using Xunit;

    public class CallBankApiTests
    {
        private class Arrangements
        {
            private readonly IServiceProvider serviceProvider;
            public readonly ProcessPayment Cmd;
            public readonly CallBankApi SUT;

            public Arrangements(IServiceProvider serviceProvider, ProcessPayment cmd)
            {
                this.serviceProvider = serviceProvider;
                this.Cmd = cmd;
                SUT = new CallBankApi(serviceProvider);
            }
        }

        private class ArrangementsBuilder
        {
            private ProcessPayment cmd;
            private Mock<IAcquiringBank> acquiringBankMock;
            private Mock<IBankFactory> bankFactoryMock;
            IServiceProvider serviceProvider;

            public ArrangementsBuilder WithACommandWhichResultsInSuccess()
            {
                cmd = InitialiseAProcessPaymentCommand();
                acquiringBankMock = new Mock<IAcquiringBank>();
                bankFactoryMock = new Mock<IBankFactory>();
                acquiringBankMock.Setup(x => x.ProcessPayment(cmd.CardNumber,
                        cmd.Cvv, cmd.ExpiryDate, cmd.Amount, cmd.Currency, cmd.MerchantId))
                    .Returns((Guid.NewGuid(), "SUCCESS"));
                bankFactoryMock.Setup(x => x.GetBank(cmd.MerchantId)).Returns(acquiringBankMock.Object);

                SetupServiceProvider();
                return this;
            }
            
            public ArrangementsBuilder WithACommandWhichResultsInFailure()
            {
                cmd = InitialiseAProcessPaymentCommand();
                acquiringBankMock = new Mock<IAcquiringBank>();
                bankFactoryMock = new Mock<IBankFactory>();
                acquiringBankMock.Setup(x => x.ProcessPayment(cmd.CardNumber,
                        cmd.Cvv, cmd.ExpiryDate, cmd.Amount, cmd.Currency, cmd.MerchantId))
                    .Returns((Guid.NewGuid(), "UNSUCCESSFUL"));
                bankFactoryMock.Setup(x => x.GetBank(cmd.MerchantId)).Returns(acquiringBankMock.Object);

                SetupServiceProvider();
                return this;
            }
            
            public ArrangementsBuilder WithACommandWhichResultsInSystemError()
            {
                cmd = InitialiseAProcessPaymentCommand();
                acquiringBankMock = new Mock<IAcquiringBank>();
                bankFactoryMock = new Mock<IBankFactory>();
                acquiringBankMock.Setup(x => x.ProcessPayment(cmd.CardNumber,
                        cmd.Cvv, cmd.ExpiryDate, cmd.Amount, cmd.Currency, cmd.MerchantId))
                    .Throws(new Exception("An exception"));
                bankFactoryMock.Setup(x => x.GetBank(cmd.MerchantId)).Returns(acquiringBankMock.Object);

                SetupServiceProvider();
                return this;
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
                return msg;
            }
            private void SetupServiceProvider()
            {
                var serviceCollection = new ServiceCollection();
                serviceCollection.AddScoped<IBankFactory>(provider => bankFactoryMock.Object);
                serviceProvider = serviceCollection.BuildServiceProvider();
            }

            public Arrangements Build()
            {
                return new Arrangements(serviceProvider,cmd);
            }
        }
        
        [Fact]
        public void CallBank_WhenPaymentIsSuccessful_ReturnsSuccessMessage()
        {
            // Arrange
            var arrangements = new ArrangementsBuilder()
                .WithACommandWhichResultsInSuccess()
                .Build();
            
            // Act
            (Guid paymentResponseId, string paymentMessage) result = arrangements.SUT.CallBank(arrangements.Cmd);
            
            // Assert
            result.paymentMessage.Should().Be("SUCCESS");
        }
        
        // [Fact]
        // public void CallBank_WhenSystemError_ReturnsSystemErrorMessage()
        // {
        //     // Arrange
        //     var arrangements = new ArrangementsBuilder()
        //         .WithACommandWhichResultsInSystemError()
        //         .Build();
        //     
        //     // Act
        //     (Guid paymentResponseId, string paymentMessage) result = arrangements.SUT.CallBank(arrangements.Cmd);
        //     
        //     // Assert
        //     result.paymentMessage.Should().Contain("System error:");
        // }
    }
}