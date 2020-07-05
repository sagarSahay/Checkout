namespace AcquiringBank.API.Models
{
    using System;

    public class BankResponse
    {
        public string Message { get; set; }
        public Guid PaymentResponseId { get; set; }
    }
}