namespace PaymentGateway.WriteModel.Application
{
    public interface IBankFactory
    {
        public IAcquiringBank GetBank(string merchantId);
    }
}