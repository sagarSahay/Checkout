using PaymentGateway.Events.v1;

namespace PaymentGateway.WriteModel.Application.Messages.CommandHandlers
{
    using System;
    using System.Threading.Tasks;
    using Commands;
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
            var bankFactory = serviceProvider.GetService<IBankFactory>();
            var bank = bankFactory.GetBank(command.MerchantId);

            var bankApiResult = (paymentResponseId:Guid.Empty, paymentMessage:string.Empty);
            try
            {
                bankApiResult = bank.ProcessPayment(
                    command.CardNumber,
                    command.Cvv, command.ExpiryDate,
                    command.Amount,
                    command.Currency,
                    command.MerchantId);
            }
            catch (Exception e)
            {
               var paymentError = new PaymentError()
               {
                   Amount = command.Amount,
                   MerchantId = command.MerchantId,
                   CardNumber = TrimCardNumber(command.CardNumber),
                   Currency = command.Currency,
                   OrderId = command.OrderId,
                   PaymentId = command.PaymentId,
                   Error = e.Message
               };
               await publishEndpoint.Publish(paymentError);
               return;
            }
            //var (paymentResponseId, paymentMessage) = (Guid.NewGuid(), "SUCCESS");
           

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
                    MerchantId = command.MerchantId
                };

                await publishEndpoint.Publish(msg);
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
                    MerchantId = command.MerchantId
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