namespace PaymentGateway.ReadModel.Denormalizer.DbRepositoryContracts
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using DocumentContracts;

    public interface IDocumentStore : IDisposable
    {
        Task CreateCollection(string collectionId);

        Task<int> Count(string collectionId);

        Task<IEnumerable<T>> ReadAll<T>(string collectionId);
        
        Task<DocumentBase<T>> ReadDocument<T>(string collectionId, string documentId);

        Task UpsertDocument<T>(string collectionId, string documentId, DocumentBase<T> item);

        Task DeleteDocument(string collectionId, string documentId);
        
        bool DocumentExists(string collectionId, string documentId);

        Task<bool> CollectionExists(string collectionId);
        
        Task ClearCollection(string collectionId);
    }
}