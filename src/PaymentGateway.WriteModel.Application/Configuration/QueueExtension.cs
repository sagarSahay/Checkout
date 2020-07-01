namespace PaymentGateway.WriteModel.Application.Configuration
{
    using System;
    using MassTransit;
    using Messages.CommandHandlers;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

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
            }));


            return services;
        }
    }
}
