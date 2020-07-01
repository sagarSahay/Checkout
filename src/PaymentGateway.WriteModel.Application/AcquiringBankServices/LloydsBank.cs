namespace PaymentGateway.WriteModel.Application
{
    using System;

    public class LloydsBank : IAcquiringBank
    {
        public (Guid, string) ProcessPayment(string cardNumber, string cvv, string expiryDate, decimal amount, string currency)
        {
            throw new NotImplementedException();
        }
    }
}