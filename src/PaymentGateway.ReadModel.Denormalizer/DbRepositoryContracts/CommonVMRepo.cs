namespace PaymentGateway.ReadModel.Denormalizer.DbRepositoryContracts
{
    public abstract class CommonVMRepo<VM> : DocumentDbRepository<VM>
    {
        protected CommonVMRepo(IDocumentStore db, string collectionName = null) : base(db, collectionName)
        {
        }
    }
}