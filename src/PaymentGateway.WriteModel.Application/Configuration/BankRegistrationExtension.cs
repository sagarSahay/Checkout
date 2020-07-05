namespace PaymentGateway.WriteModel.Application.Configuration
{
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

    public static class BankRegistrationExtension
    {
        public static IServiceCollection RegisterAcquiringBanks(this IServiceCollection services,
            HostBuilderContext context)
        {
            var bankSettings = new BankSettings();

            context.Configuration.GetSection("BankSettings").Bind(bankSettings);

            services.AddSingleton(bankSettings);
            services.AddScoped<IBankFactory, AcquiringBankFactory>();
            return services;
        }
    }
}
