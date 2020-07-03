namespace PaymentGateway.ReadModel.Denormalizer.PaymentRepository
{
    using DbRepositoryContracts;

    internal class PaymentRepository : CommonVMRepo<PaymentVM>,
        IPaymentQueryRepository
    {
        public PaymentRepository(IDocumentStore db)
            : base(db, typeof(PaymentVM).FullName)
        {
        }
    }
}