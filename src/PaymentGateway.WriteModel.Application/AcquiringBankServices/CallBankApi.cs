namespace PaymentGateway.WriteModel.Application.AcquiringBankServices
{
    using System;
    using Commands;
    using Microsoft.Extensions.DependencyInjection;

    public class CallBankApi : ICallBankApi
    {
        private readonly IServiceProvider serviceProvider;

        public CallBankApi(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public (Guid, string) CallBank(ProcessPayment command)
        {
            var bankFactory = serviceProvider.GetService<IBankFactory>();
            var bank = bankFactory.GetBank(command.MerchantId);

            (Guid paymentResponseId, string paymentMessage) bankApiResult;
            try
            {
                // bankApiResult = bank.ProcessPayment(
                //     command.CardNumber,
                //     command.Cvv, command.ExpiryDate,
                //     command.Amount,
                //     command.Currency,
                //     command.MerchantId);
                
                // TODO: Fix acquiring bank api in docker compose
                bankApiResult = (Guid.NewGuid(), "SUCCESS");
            }
            catch (Exception e)
            {
                return (Guid.Empty, $"System error: {e.Message}");
            }

            return bankApiResult;
        }
    }
}