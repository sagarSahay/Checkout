using System;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PaymentGateway.WriteModel.Application.Messages.CommandHandlers;


namespace PaymentGateway.WriteModel.Application.Configuration
{
    public static class QueueExtension
    {
        public static IServiceCollection RegisterQueueServices(this IServiceCollection services, HostBuilderContext context)
        {
            var queueSettings = new QueueSettings();
            context.Configuration.GetSection("QueueSettings").Bind(queueSettings);

            services.AddMassTransit(c =>
            {
                c.AddConsumer<ProcessPaymentHandler>();
            });

            services.AddSingleton(provider => Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
               
                cfg.Host(new Uri(queueSettings.HostName), h => {
                    h.Username(queueSettings.Username);
                    h.Password(queueSettings.Password);
                });

                // cfg.ReceiveEndpoint(queueSettings.ReceiveQueueName, e =>
                //{
                //    e.Consumer<ProcessPaymentHandler>(provider);
                //});
            }));


            return services;
        }
    }
}
