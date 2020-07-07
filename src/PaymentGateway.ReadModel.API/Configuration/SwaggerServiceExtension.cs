namespace PaymentGateway.ReadModel.API.Configuration
{
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.OpenApi.Models;

    public static class SwaggerServiceExtension
    {
        public static IServiceCollection RegisterSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Payment Query API", Version = "v1" });
            });

            return services;
        }
    }
}