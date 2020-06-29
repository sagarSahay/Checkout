namespace PaymentGateway.WriteModel.API
{
    using System;
    using System.IO;
    using AutoMapper;
    using FluentValidation;
    using FluentValidation.AspNetCore;
    using MassTransit;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Models;
    using Validators;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAutoMapper(typeof(Startup));
            
            services.AddControllers().AddFluentValidation();
            services.AddTransient<IValidator<PaymentRequest>, PaymentRequestValidator>();
            
            ConfigureRabbit(services);
        }

        private static void ConfigureRabbit(IServiceCollection services)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json").Build();

            var rabbitHost = config["RabbitMqHost"];
            var rabbitUser = config["RabbitMqUser"];
            var rabbitPassword = config["RabbitMqPassword"];
            var commandQueue = config["CommandQueue"];

            services.AddMassTransit();
            services.AddSingleton(x =>
                {
                    var bus = Bus.Factory.CreateUsingRabbitMq(sbc =>
                    {
                        sbc.Host(new Uri(rabbitHost), h =>
                        {
                            h.Username(rabbitUser);
                            h.Password(rabbitPassword);
                        });
                    });

                    return bus.GetSendEndpoint(new Uri($"{rabbitHost}/{commandQueue}")).Result;
                }
            );

            services.AddMassTransitHostedService();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();
            
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}