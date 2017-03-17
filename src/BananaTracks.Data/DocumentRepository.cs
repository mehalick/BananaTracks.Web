using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using BananaTracks.Entities;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;

namespace BananaTracks.Data
{
    public class DocumentRepository<T> where T : EntityBase
    {
        private const string DatabaseId = DocumentClientFactory.DatabaseId;
        private const string CollectionId = DocumentClientFactory.CollectionId;

        private readonly IDocumentClient _client;
        private readonly Uri _collectionUri;

        public DocumentRepository(IDocumentClient client)
        {
            _client = client;
            _collectionUri = UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId);
        }

        private static Uri GetDocumentUri(string id)
        {
            return UriFactory.CreateDocumentUri(DatabaseId, CollectionId, id);
        }

        public async Task<T> GetItem(string id)
        {
            try
            {
                var document = await _client.ReadDocumentAsync(GetDocumentUri(id));
                return (T)(dynamic)document;
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return null;
                }

                throw;
            }
        }

        public async Task<IReadOnlyCollection<T>> GetItems(Expression<Func<T, bool>> predicate)
        {
            var feedOptions = new FeedOptions { MaxItemCount = -1 };

            var query = _client.CreateDocumentQuery<T>(_collectionUri, feedOptions)
                .Where(i => i.Type == typeof(T).ToString())
                .Where(predicate)
                .AsDocumentQuery();

            var results = new List<T>();
            while (query.HasMoreResults)
            {
                results.AddRange(await query.ExecuteNextAsync<T>());
            }

            return results;
        }

        public async Task<T> CreateItem(T item)
        {
            item.CreatedUtc = DateTime.UtcNow;

            var document = await _client.CreateDocumentAsync(_collectionUri, item);

            return (T)(dynamic)document.Resource;
        }

        public async Task<Document> UpdateItem(string id, T item)
        {
            return await _client.ReplaceDocumentAsync(GetDocumentUri(id), item);
        }

        public async Task DeleteItem(string id)
        {
            await _client.DeleteDocumentAsync(GetDocumentUri(id));
        }

        //public static void Initialize()
        //{
        //    _client = new DocumentClient(new Uri(Endpoint), Key, new ConnectionPolicy { EnableEndpointDiscovery = false });
        //    CreateDatabaseIfNotExistsAsync().Wait();
        //    CreateCollectionIfNotExistsAsync().Wait();
        //}

        //private static async Task CreateDatabaseIfNotExistsAsync()
        //{
        //    try
        //    {
        //        await _client.ReadDatabaseAsync(UriFactory.CreateDatabaseUri(DatabaseId));
        //    }
        //    catch (DocumentClientException e)
        //    {
        //        if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
        //        {
        //            await _client.CreateDatabaseAsync(new Database { Id = DatabaseId });
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }
        //}

        //private static async Task CreateCollectionIfNotExistsAsync()
        //{
        //    try
        //    {
        //        await _client.ReadDocumentCollectionAsync(UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId));
        //    }
        //    catch (DocumentClientException e)
        //    {
        //        if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
        //        {
        //            await _client.CreateDocumentCollectionAsync(
        //                UriFactory.CreateDatabaseUri(DatabaseId),
        //                new DocumentCollection { Id = CollectionId },
        //                new RequestOptions { OfferThroughput = 400 });
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }
        //}
    }
}
