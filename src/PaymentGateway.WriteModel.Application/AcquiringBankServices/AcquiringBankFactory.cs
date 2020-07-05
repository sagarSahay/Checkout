namespace PaymentGateway.WriteModel.Application
{
    using System;
    using AcquiringBankServices;

    public class AcquiringBankFactory : IBankFactory
    {
        private readonly BankSettings settings;

        public AcquiringBankFactory(BankSettings settings)
        {
            this.settings = settings;
        }

        public IAcquiringBank GetBank(string merchantId)
        {
            if (merchantId == "merchant1")
            {
                return new LloydsBank(settings);
            }
            return  new BarclaysBank(settings);
        }
    }
}