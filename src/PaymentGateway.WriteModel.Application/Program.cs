namespace PaymentGateway.WriteModel.Application
{
    using Configuration;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

    public class Program
    {
        public static void Main()
        {
            CreateHostBuilder().Build().Run();
        }

        public static IHostBuilder CreateHostBuilder() =>
            Host.CreateDefaultBuilder()
                .UseWindowsService()
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.AddConfiguration(new ConfigurationBuilder().AddJsonFile("appsettings.json").Build());
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.RegisterConfigurationServices(hostContext);
                    services.RegisterQueueServices(hostContext);
                    services.RegisterAcquiringBanks(hostContext);
                    services.AddHostedService<Worker>();
                });
    }
}