namespace PaymentGateway.WriteModel.Application.Configuration
{
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

    public static class BankRegistrationExtension
    {
        public static IServiceCollection RegisterAcquiringBanks(this IServiceCollection services,
            HostBuilderContext context)
        {
            services.AddScoped<IBankFactory, AcquiringBankFactory>();
            return services;
        }
    }
}
