namespace PaymentGateway.Messages.Common
{
    using System;

    public class Message :IMessage
    {
        public Guid Id { get; } = Guid.NewGuid();
    }
}