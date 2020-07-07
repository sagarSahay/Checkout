namespace PaymentGateway.WriteModel.Application.AcquiringBankServices
{
    using System;
    using Commands;

    public interface ICallBankApi
    {
        public (Guid, string) CallBank(ProcessPayment cmd);
    }
}