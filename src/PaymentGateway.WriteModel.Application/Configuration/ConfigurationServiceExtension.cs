namespace PaymentGateway.WriteModel.Application.Configuration
{
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

    public static class ConfigurationServiceExtension
    {
        public static IServiceCollection RegisterConfigurationServices(this IServiceCollection service,
            HostBuilderContext context)
        {
            var queueSettings = new QueueSettings();

            context.Configuration.GetSection("QueueSettings").Bind(queueSettings);

            service.AddSingleton(queueSettings);

            return service;
        }
    }
}
