namespace PaymentGateway.ReadModel.Denormalizer.Configuration
{
    using DbRepositoryContracts;
    using Handlers;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using MongoDb;
    using PaymentRepository;

    public static class RepositoryExtension
    {
        public static IServiceCollection RegisterDenormalizer(this IServiceCollection services,
            HostBuilderContext context)
        {
            var mongoSettings = new MongoSettings();

            context.Configuration.GetSection("MongoSettings").Bind(mongoSettings);
            services.AddSingleton(mongoSettings);

            services.AddSingleton<PaymentDenormalizer>();
            services.AddSingleton<IDocumentStore, MongoDb>();
            services.AddSingleton<IDenormalizerRepository<PaymentVM>>(provider =>
                new PaymentRepository(provider.GetRequiredService<IDocumentStore>()));
            //services.AddSingleton(provider => new PaymentRepository(provider.GetRequiredService<IDocumentStore>()));
            return services;
        }
    }
}