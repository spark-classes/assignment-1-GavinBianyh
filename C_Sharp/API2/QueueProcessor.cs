using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Azure.Security.KeyVault.Secrets;
using Azure.Identity;

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
        public void Run([QueueTrigger("api1queue-poison", Connection = "newsetting")]string myQueueItem, ILogger log)
        {
            // Decode the Base64 string before processing it
            string decodedMessage = Base64Decode(myQueueItem);

            log.LogInformation($"C# Queue trigger function processed: {decodedMessage}");

            // Read the third secret from Azure Key Vault
            KeyVaultSecret thirdSecret = _secretClient.GetSecret("secret3");
            log.LogInformation($"Third Secret Value: {thirdSecret.Value}");
        }

        private static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        private static string Base64Decode(string base64EncodedText)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedText);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }
    }
}
