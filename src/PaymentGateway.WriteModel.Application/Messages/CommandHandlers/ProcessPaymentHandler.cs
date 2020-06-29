namespace PaymentGateway.WriteModel.Application.Messages.CommandHandlers
{
    using System.Threading.Tasks;
    using Commands;
    using MassTransit;

    public class ProcessPaymentHandler : IConsumer<ProcessPayment>
    {
        public Task Consume(ConsumeContext<ProcessPayment> context)
        {
            //var command = context.Message;
            return Task.FromResult(0);
        }
    }
}