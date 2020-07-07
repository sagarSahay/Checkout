namespace PaymentGateway.ReadModel.Denormalizer
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Configuration;
    using Handlers;
    using MassTransit;
    using Microsoft.Extensions.Hosting;

    public class Worker : BackgroundService
    {
        private readonly IBusControl busControl;
        private readonly QueueSettings queueSettings;
        private readonly IServiceProvider serviceProvider;

        public Worker(IBusControl busControl, QueueSettings queueSettings, IServiceProvider serviceProvider)
        {
            this.busControl = busControl;
            this.queueSettings = queueSettings;
            this.serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var paymentDenormalizer = busControl.ConnectReceiveEndpoint(queueSettings.ReceiveQueueName, x =>
            {
                x.Consumer<PaymentDenormalizer>(serviceProvider);
            });

            await paymentDenormalizer.Ready;
        }
    }
}