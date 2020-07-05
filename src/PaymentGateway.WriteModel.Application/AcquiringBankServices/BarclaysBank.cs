namespace PaymentGateway.WriteModel.Application
{
    using System;
    using AcquiringBankServices;
    using RestSharp;

    public class BarclaysBank : IAcquiringBank
    {
        private readonly BankSettings settings;

        public BarclaysBank(BankSettings settings)
        {
            this.settings = settings;
        }

        public (Guid, string) ProcessPayment(string cardNumber, string cvv, string expiryDate, decimal amount, string currency, string merchantId)
        {
            var bankRequest = new BankCardRequest()
            {
                Amount = amount,
                CardNumber = cardNumber,
                Currency = currency,
                Cvv = cvv,
                ExpiryDate = expiryDate,
                MerchantId = merchantId
            };

            var client = new RestClient(settings.BarclaysBank);

            var request = new RestRequest("/process-payment/", Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddJsonBody(bankRequest);

            var response = client.Post<BankResponse>(request);

            return (response.Data.PaymentResponseId, response.Data.Message);
        }
    }
}