namespace PaymentGateway.Commands
{
    using System;
    using Messages.Common;

    public class ProcessPayment : Command
    {
        public Guid PaymentId { get; set; }
        public string CardNumber { get; set; }
        public string Cvv { get; set; }
        public string ExpiryDate { get; set; }
        public string OrderId { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public string MerchantId { get; set; }
    }
}