namespace PaymentGateway.WriteModel.Application
{
    using System.Threading.Tasks;
    using Configuration;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;


    public class Program
    {
        public static void Main()
        {
            //var services = new ServiceCollection();

            //services.AddHttpClient();

            //services.AddScoped<AcquiringBankFactory>();

            //services.AddScoped<LloydsBank>()
            //    .AddScoped<IAcquiringBank, LloydsBank>(s => s.GetService<LloydsBank>());

            //services.AddScoped<BarclaysBank>()
            //    .AddScoped<IAcquiringBank, BarclaysBank>(s => s.GetService<BarclaysBank>());

            //var config = new ConfigurationBuilder()
            //    .SetBasePath(Directory.GetCurrentDirectory())
            //    .AddJsonFile("appsettings.json").Build();

            //await ConfigureRabbitMq(config, services);

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
                    services.AddHostedService<Worker>();
                });
    }
}