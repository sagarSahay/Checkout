namespace PaymentGateway.Events.v1
{
    using System;
    using Messages.Common;

    public class PaymentSuccessful : IEvent
    {
        public Guid Id => Guid.NewGuid();
        public string CardNumber { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public string OrderId { get; set; }
        public Guid PaymentId { get; set; }
        public string PaymentResponseId { get; set; }
        public string PaymentResponseStatus { get; set; }

        public string Cvv { get; set; }
        public string ExpiryDate { get; set; }

        public string MerchantId { get; set; }
    }
}