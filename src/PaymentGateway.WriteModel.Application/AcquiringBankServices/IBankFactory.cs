namespace PaymentGateway.WriteModel.Application
{
    using AcquiringBankServices;

    public interface IBankFactory
    {
        public IAcquiringBank GetBank(string merchantId);
    }
}