using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Hosting.WindowsServices;
using PaymentGateway.WriteModel.Application.Configuration;

namespace PaymentGateway.WriteModel.Application
{
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;
    using MassTransit;
    using Messages.CommandHandlers;
    using Microsoft.Extensions.Configuration;
    using Console = System.Console;
    using TimeSpan = System.TimeSpan;
    using Uri = System.Uri;


    public class Program
    {
        public static async Task Main()
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