namespace PaymentGateway.ReadModel.Denormalizer.PaymentRepository
{
    using System;

    public class PaymentVM
    {
        public string CardNumber { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public string OrderId { get; set; }
        public Guid PaymentId { get; set; }
        public string PaymentResponseId { get; set; }
        public string PaymentResponseStatus { get; set; }
    }
}