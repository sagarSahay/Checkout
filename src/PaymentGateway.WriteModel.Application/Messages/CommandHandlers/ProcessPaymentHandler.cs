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
        private readonly IServiceProvider _serviceProvider;

        public ProcessPaymentHandler(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task Consume(ConsumeContext<ProcessPayment> context)
        {
            var command = context.Message;
            var publishEndpoint = _serviceProvider.GetService<IPublishEndpoint>();
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