using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace API2
{
    public class QueueProcessor
    {
        private readonly SecretClient _secretClient;

        public QueueProcessor()
        {
            var kvUri = "https://keybyh.vault.azure.net"; 
            _secretClient = new SecretClient(new Uri(kvUri), new DefaultAzureCredential());
        }

        [FunctionName("QueueProcessor")]
        public void Run([QueueTrigger("api1queue", Connection = "newsetting")]string myQueueItem, ILogger log)
        {
            log.LogInformation($"C# Queue trigger function processed: {myQueueItem}");

            // Read the third secret from Azure Key Vault
            KeyVaultSecret thirdSecret = _secretClient.GetSecret("secret3");
            log.LogInformation($"Third Secret Value: {secret3.Value}");
        }
    }
}
