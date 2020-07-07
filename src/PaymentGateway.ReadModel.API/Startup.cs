namespace PaymentGateway.ReadModel.API
{
    using Configuration;
    using Denormalizer.PaymentRepository;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using Serilog;

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
            services.RegisterSwagger();
            services.AddMvc();
            services.AddControllers();
            services.RegisterCorsPolicy();
            services.RegisterLogging();
            services.RegisterRepository(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,  ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("DefaultPolicy");
            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseSwaggerConfiguration();

            app.UseAuthorization();

            loggerFactory.AddSerilog();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}