namespace PaymentGateway.WriteModel.Application.Messages.CommandHandlers
{
    using System;
    using System.Threading.Tasks;
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
            var bankFactory = serviceProvider.GetService<AcquiringBankFactory>();
            var bank = bankFactory.GetBank(command.MerchantId);

            var (paymentResponseId, paymentMessage) = bank.ProcessPayment(
                command.CardNumber,
                command.Cvv, command.ExpiryDate,
                command.Amount,
                command.Currency);

            if (paymentMessage == "SUCCESS")
            {
                var msg = new PaymentSuccessful()
                {
                    CardNumber = command.CardNumber,
                    Amount = command.Amount,
                    Currency = command.Currency,
                };

                await publishEndpoint.Publish(msg);
            }
            else
            {
                var msg = new PaymentUnsuccessful()
                {
                    CardNumber = command.CardNumber,
                    Amount = command.Amount,
                    Currency = command.Currency,
                    ErrorMessage = paymentMessage
                };

                await publishEndpoint.Publish(msg);
            }
        }
    }
}