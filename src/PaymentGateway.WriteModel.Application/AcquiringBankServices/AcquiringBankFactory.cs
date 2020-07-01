namespace PaymentGateway.WriteModel.Application
{
    using System;

    public class AcquiringBankFactory : IBankFactory
    {

        public IAcquiringBank GetBank(string merchantId)
        {
            if (merchantId == "merchant1")
            {
                return new LloydsBank();
            }
            return  new BarclaysBank();
        }
    }
}