namespace PaymentGateway.WriteModel.Application.Messages.CommandHandlers
{
    using System;
    using System.Threading.Tasks;
    using AcquiringBankServices;
    using Commands;
    using Events.v1;
    using MassTransit;
    using Microsoft.Extensions.DependencyInjection;

    public class ProcessPaymentHandler : IConsumer<ProcessPayment>
    {
        private readonly IServiceProvider serviceProvider;

        public ProcessPaymentHandler(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public async Task Consume(ConsumeContext<ProcessPayment> context)
        {
            var command = context.Message;
            var publishEndpoint = serviceProvider.GetService<IPublishEndpoint>();
            var bankService = serviceProvider.GetService<ICallBankApi>();
            (Guid paymentResponseId, string paymentMessage) bankApiResult = bankService.CallBank(command);
            //TODO: Add event store 
            if (bankApiResult.paymentMessage == "SUCCESS")
            {
                var msg = new PaymentSuccessful()
                {
                    CardNumber = TrimCardNumber(command.CardNumber),
                    Amount = command.Amount,
                    Currency = command.Currency,
                    PaymentId = command.PaymentId,
                    PaymentResponseId = bankApiResult.paymentResponseId.ToString(),
                    PaymentResponseStatus = bankApiResult.paymentMessage,
                    OrderId = command.OrderId,
                    MerchantId = command.MerchantId,
                    Cvv = command.Cvv,
                    ExpiryDate = command.ExpiryDate
                };

                await publishEndpoint.Publish(msg);
            }
            else if (bankApiResult.paymentMessage.Contains("System error"))
            {
                var paymentError = new PaymentError()
                {
                    Amount = command.Amount,
                    MerchantId = command.MerchantId,
                    CardNumber = TrimCardNumber(command.CardNumber),
                    Currency = command.Currency,
                    OrderId = command.OrderId,
                    PaymentId = command.PaymentId,
                    Error = bankApiResult.paymentMessage,
                    Cvv = command.Cvv,
                    ExpiryDate = command.ExpiryDate
                };
                await publishEndpoint.Publish(paymentError);
            }
            else
            {
                var msg = new PaymentUnsuccessful()
                {
                    CardNumber = TrimCardNumber(command.CardNumber),
                    Amount = command.Amount,
                    Currency = command.Currency,
                    ErrorMessage = bankApiResult.paymentMessage,
                    OrderId = command.OrderId,
                    MerchantId = command.MerchantId,
                    Cvv = command.Cvv,
                    ExpiryDate = command.ExpiryDate
                };

                await publishEndpoint.Publish(msg);
            }
        }

        private string TrimCardNumber(string cardNumber)
        {
            return "****-****-****-" + cardNumber.Substring(cardNumber.Length - 4);
        }
    }
}