using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using System;
using System.Threading.Tasks;

namespace BananaTracks.Data
{
    public static class DocumentClientFactory
    {
        public static async Task<IDocumentClient> CreateClient(string endpoint, string authKey)
        {
            var client = new DocumentClient(new Uri(endpoint), authKey, new ConnectionPolicy
            {
                ConnectionMode = ConnectionMode.Direct,
                ConnectionProtocol = Protocol.Tcp,
                EnableEndpointDiscovery = false
            });

            await CreateDatabaseIfNotExistsAsync(client);
            await CreateCollectionIfNotExistsAsync(client);

            return client;
        }

        private static async Task CreateDatabaseIfNotExistsAsync(IDocumentClient client)
        {
            try
            {
                await client.ReadDatabaseAsync(UriFactory.CreateDatabaseUri(DatabaseSettings.DatabaseId));
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    await client.CreateDatabaseAsync(new Database { Id = DatabaseSettings.DatabaseId });
                }
                else
                {
                    throw;
                }
            }
        }

        private static async Task CreateCollectionIfNotExistsAsync(IDocumentClient client)
        {
            try
            {
                await client.ReadDocumentCollectionAsync(UriFactory.CreateDocumentCollectionUri(DatabaseSettings.DatabaseId, DatabaseSettings.CollectionId));
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    await client.CreateDocumentCollectionAsync(
                        UriFactory.CreateDatabaseUri(DatabaseSettings.DatabaseId),
                        new DocumentCollection { Id = DatabaseSettings.CollectionId },
                        new RequestOptions { OfferThroughput = 400 });
                }
                else
                {
                    throw;
                }
            }
        }
    }
}
