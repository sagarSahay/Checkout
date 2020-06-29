namespace PaymentGateway.WriteModel.API.Models
{
    using System;

    public class PaymentRequest 
    {
        public string CardNumber { get; set; }
        public string Cvv { get; set; }
        public string ExpiryDate { get; set; }
        public string OrderId { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        
        public string MerchantId { get; set; }
    }
}