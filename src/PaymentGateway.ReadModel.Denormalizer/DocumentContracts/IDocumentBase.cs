namespace PaymentGateway.ReadModel.Denormalizer.DocumentContracts
{
    public interface IDocumentBase
    {
        string ETag { get; set; }
    }

    public interface IDocumentBase<T> : IDocumentBase
    {
        T VM { get; set; }
    }
}