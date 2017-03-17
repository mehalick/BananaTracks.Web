using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using System;

namespace BananaTracks.Data
{
    public static class DocumentClientFactory
    {
        public static IDocumentClient CreateClient(string endpoint, string authKey)
        {
            var client = new DocumentClient(new Uri(endpoint), authKey, new ConnectionPolicy
            {
                ConnectionMode = ConnectionMode.Direct,
                ConnectionProtocol = Protocol.Tcp,
                EnableEndpointDiscovery = false
            });

            client.OpenAsync().Wait();

            return client;
        }
    }
}
