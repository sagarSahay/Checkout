namespace PaymentGateway.ReadModel.Denormalizer.Configuration
{
    using System;
    using Handlers;
    using MassTransit;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

    public static class QueueExtension
    {
        public static IServiceCollection RegisterQueueServices(this IServiceCollection services,
            HostBuilderContext context)
        {
            var queueSettings = new QueueSettings();
            context.Configuration.GetSection("QueueSettings").Bind(queueSettings);

            services.AddMassTransit(c =>
            {
                c.AddConsumer<PaymentDenormalizer>();
            });

            services.AddSingleton(provider => Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                cfg.Host(queueSettings.HostName,"/", h =>
                {
                    h.Username(queueSettings.Username);
                    h.Password(queueSettings.Password);
                });
            }));


            return services;
        }
    }
}