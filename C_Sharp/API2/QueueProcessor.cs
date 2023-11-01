using Azure.Security.KeyVault.Secrets;
using Azure.Identity;

namespace API2
{
    public class QueueProcessor
    {
        string storageConnectionString = Environment.GetEnvironmentVariable("AZURE_STORAGE_CONNECTION_STRING");


        [FunctionName("QueueProcessor")]
        public void Run([QueueTrigger("api1queue", Connection = "newsetting")]string myQueueItem, ILogger log)
        {
            var client = new SecretClient(new Uri(kvUri), new DefaultAzureCredential());
            KeyVaultSecret secret3 = client.GetSecret("secret3");
            log.LogInformation($"Third secret's value: {secret3.Value}");
            
            log.LogInformation($"C# Queue trigger function processed: {myQueueItem}");
        }
    }
}
