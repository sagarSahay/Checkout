namespace PaymentGateway.Events.v1
{
    using System;
    using Messages.Common;

    public class PaymentError: IEvent
    {
        public Guid Id => Guid.NewGuid();
        public string CardNumber { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public string OrderId { get; set; }
        public Guid PaymentId { get; set; }
        public string Error { get; set; }

        public string MerchantId { get; set; }
    }
}