namespace PaymentGateway.WriteModel.Application
{
    using System;

    public interface IAcquiringBankInterface
    {
        (Guid, string) ProcessPayment(string cardNumber, string cvv, string expiryDate, string amount, string currency);
    }
}