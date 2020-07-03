namespace PaymentGateway.ReadModel.Denormalizer.DbRepositoryContracts
{
    internal abstract class CommonVMRepo<VM> : DocumentDbRepository<VM>
    {
        protected CommonVMRepo(IDocumentStore db, string collectionName = null) : base(db, collectionName)
        {
        }
    }
}