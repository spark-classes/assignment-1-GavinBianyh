using System;
using System.Threading.Tasks;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace API2
{
    public class QueueProcessor
    {
        [FunctionName("QueueProcessor")]
        public void Run([QueueTrigger("api1queue", Connection = "StorageQueueUrl")]string myQueueItem, ILogger log)
        {
            log.LogInformation($"C# Queue trigger function processed: {myQueueItem}");
        }

        [FunctionName("GetThirdSecret")]
        public async Task<IActionResult> GetThirdSecret(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "secrets/secret3")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("Fetching secret3 from Key Vault.");
            string secretValue = await GetSecretFromKeyVaultAsync();
            return new OkObjectResult(secretValue);
        }

        private async Task<string> GetSecretFromKeyVaultAsync()
        {
            var kvUri = "https://keybyh.vault.azure.net";
            var client = new SecretClient(new Uri(kvUri), new DefaultAzureCredential());

            KeyVaultSecret secret = await client.GetSecretAsync("secret3");

            return secret.Value;
        }
    }
}
