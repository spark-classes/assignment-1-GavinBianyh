using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Azure.Security.KeyVault.Secrets;
using Azure.Identity;
using Azure.Storage.Queues; // Added for QueueClient
using Azure.Storage.Queues.Models; // Added for QueueClientOptions

namespace API2
{
    public class QueueProcessor
    {
        private readonly SecretClient _secretClient;
        private readonly QueueClient _queue; // Added for QueueClient

        public QueueProcessor()
        {
            var kvUri = "https://keybyh.vault.azure.net"; 
            _secretClient = new SecretClient(new Uri(kvUri), new DefaultAzureCredential());

            // Initialize QueueClient
            string connectionString = "DefaultEndpointsProtocol=https;AccountName=cs210032002f776443c;AccountKey=TRt7H5Yt2GSCxGj+obrW0tU4X8iK5MtLnAmECYBKWEJlnij4UCyQtftVJ2n0XdE+3kxQGWvihkPt+ASt5+QCww==;EndpointSuffix=core.windows.net"; // Replace this with your connection string
            string queueName = "api1queue-poison"; // You mentioned this queue name in your function trigger
            _queue = new QueueClient(connectionString, queueName, new QueueClientOptions
            {
                MessageEncoding = QueueMessageEncoding.Base64
            });
        }

        [FunctionName("QueueProcessor")]
        public void Run([QueueTrigger("api1queue-poison", Connection = "newsetting")]string myQueueItem, ILogger log)
        {
            log.LogInformation($"C# Queue trigger function processed: {myQueueItem}");

            // Read the third secret from Azure Key Vault
            KeyVaultSecret thirdSecret = _secretClient.GetSecret("secret3");
            log.LogInformation($"Third Secret Value: {thirdSecret.Value}");
        }

    }
}
