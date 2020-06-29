namespace PaymentGateway.Messages.Common
{
    using System;

    public interface IMessage
    {
        Guid Id { get; }
    }
}