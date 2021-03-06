namespace PaymentGateway.WriteModel.Application.AcquiringBankServices
{
    using System;

    public interface IAcquiringBank
    {
        (Guid, string) ProcessPayment(string cardNumber, string cvv, string expiryDate, decimal amount, string currency, string merchantId);
    }
}