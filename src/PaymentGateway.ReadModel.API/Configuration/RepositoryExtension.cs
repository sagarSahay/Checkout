namespace PaymentGateway.ReadModel.API.Configuration
{
    using Denormalizer.DbRepositoryContracts;
    using Denormalizer.MongoDb;
    using Denormalizer.PaymentRepository;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    public static class RepositoryExtension
    {
        public static IServiceCollection RegisterRepository(this IServiceCollection service,
            IConfiguration section)
        {

            var mongoSettingsSection = section.GetSection("MongoSettings");
            var mongoSettings = mongoSettingsSection.Get<MongoSettings>();

            service.AddSingleton(mongoSettings);
            service.AddSingleton<IDocumentStore, MongoDb>();

            service.AddSingleton<IPaymentQueryRepository, PaymentRepository>();
            return service;
        }
    }
}