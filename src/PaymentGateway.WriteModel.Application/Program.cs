using Microsoft.Extensions.DependencyInjection;

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
            var services = new ServiceCollection();

            services.AddHttpClient();

            services.AddScoped<AcquiringBankFactory>();

            services.AddScoped<LloydsBank>()
                .AddScoped<IAcquiringBank, LloydsBank>(s => s.GetService<LloydsBank>());

            services.AddScoped<BarclaysBank>()
                .AddScoped<IAcquiringBank, BarclaysBank>(s => s.GetService<BarclaysBank>());

            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json").Build();
            
            await ConfigureRabbitMq(config, services);
        }

        private static async Task ConfigureRabbitMq(IConfigurationRoot config, ServiceCollection services)
        {
            var rabbitHost = config["RabbitMqHost"];
            var rabbitUser = config["RabbitMqUser"];
            var rabbitPassword = config["RabbitMqPassword"];
            var receiveQueue = config["ReceiveQueue"];
            //services.AddSingleton(provider => Bus.Factory.CreateUsingRabbitMq(cfg =>
            //{
            //    cfg.Host(new Uri(rabbitHost), h =>
            //    {
            //        h.Username(rabbitUser);
            //        h.Password(rabbitPassword);
            //    });
            //    cfg.ReceiveEndpoint(receiveQueue, e =>
            //    {
            //        e.Consumer<ProcessPaymentHandler>(provider);
            //    });
            //}));
            //var serviceProvider = services.BuildServiceProvider();
            //var busControl = serviceProvider.GetService<IBusControl>();
            IPublishEndpoint publishEndpoint = null;
            var busControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                cfg.Host(new Uri(rabbitHost), h =>
                {
                    h.Username(rabbitUser);
                    h.Password(rabbitPassword);
                });
                cfg.ReceiveEndpoint(receiveQueue, e =>
                {
                    e.Consumer(()=> new ProcessPaymentHandler(publishEndpoint));
                });
            });
            publishEndpoint = busControl;

            var source = new CancellationTokenSource(TimeSpan.FromSeconds(10));

            await busControl.StartAsync(source.Token);
            try
            {
                Console.WriteLine("Press enter to exit");

                await Task.Run(() => Console.ReadLine());
            }
            finally
            {
                await busControl.StopAsync();
            }
        }
    }
}