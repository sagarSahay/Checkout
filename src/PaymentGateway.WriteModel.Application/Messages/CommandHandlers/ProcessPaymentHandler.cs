using PaymentGateway.Events.v1;

namespace PaymentGateway.WriteModel.Application.Messages.CommandHandlers
{
    using System.Threading.Tasks;
    using Commands;
    using MassTransit;

    public class ProcessPaymentHandler : IConsumer<ProcessPayment>
    {
        private readonly IPublishEndpoint publishEndpoint;

        public ProcessPaymentHandler(IPublishEndpoint publishEndpoint)
        {
            this.publishEndpoint = publishEndpoint;
        }

        public async Task Consume(ConsumeContext<ProcessPayment> context)
        {
            var command = context.Message;

            var msg = new PaymentSuccessful()
            {
                CardNumber = command.CardNumber,
                Amount = command.Amount,
                Currency = command.Currency,
            };

            await publishEndpoint.Publish(msg);
        }
    }
}