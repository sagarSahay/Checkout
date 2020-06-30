using System;
using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Hosting;
using PaymentGateway.WriteModel.Application.Configuration;
using PaymentGateway.WriteModel.Application.Messages.CommandHandlers;

namespace PaymentGateway.WriteModel.Application
{
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
            try
            {
                var paymentHandler = busControl.ConnectReceiveEndpoint(queueSettings.ReceiveQueueName, x =>
                {
                    x.Consumer<ProcessPaymentHandler>(serviceProvider);
                });

                await paymentHandler.Ready;
            }
            catch (Exception ex)
            {

            }
        }
    }
}
