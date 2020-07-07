namespace PaymentGateway.WriteModel.Application.Configuration
{
    using AcquiringBankServices;
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
            services.AddScoped<ICallBankApi, CallBankApi>();
            return services;
        }
    }
}
