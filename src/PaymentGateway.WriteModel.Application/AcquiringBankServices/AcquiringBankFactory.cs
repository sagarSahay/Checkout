namespace PaymentGateway.WriteModel.Application
{
    using System;

    public class AcquiringBankFactory
    {
        private readonly IServiceProvider serviceProvider;

        public AcquiringBankFactory(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public IAcquiringBank GetBank(string merchantId)
        {
            if (merchantId == "merchant1")
            {
                return (IAcquiringBank) serviceProvider.GetService(typeof(LloydsBank));
            }
            return  (IAcquiringBank) serviceProvider.GetService(typeof(BarclaysBank));
        }
    }
}