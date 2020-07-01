using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace PaymentGateway.WriteModel.Application.Configuration
{
    public static class BankRegistrationExtension
    {
        public static IServiceCollection RegisterAcquiringBanks(this IServiceCollection services,
            HostBuilderContext context)
        {
            services.AddScoped<AcquiringBankFactory>();

            services.AddScoped<LloydsBank>()
                .AddScoped<IAcquiringBank, LloydsBank>(s => s.GetService<LloydsBank>());

            services.AddScoped<BarclaysBank>()
                .AddScoped<IAcquiringBank, BarclaysBank>(s => s.GetService<BarclaysBank>());

            return services;
        }
    }
}
