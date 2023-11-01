using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Azure.Security.KeyVault.Secrets;
using Azure.Identity;
using Azure.Storage.Queues;

namespace API1
{
    public static class test1
    {
        private static readonly string kvUri = "https://keybyh.vault.azure.net/";

        [FunctionName("test1")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var client = new SecretClient(new Uri(kvUri), new DefaultAzureCredential());

            // Retrieve the secret
            KeyVaultSecret secret = await client.GetSecretAsync("secret2");
            log.LogInformation($"Secret2 = {secret.Value}");

            // Retrieve the storage connection string
            KeyVaultSecret storageConnectionStringSecret = await client.GetSecretAsync("storageConnectionString");
            string storageConnectionString = storageConnectionStringSecret.Value;

            // Use the connection string to insert a message into the queue
            QueueClient queueClient = new QueueClient(storageConnectionString, "api1queue");
            await queueClient.CreateIfNotExistsAsync();
            await queueClient.SendMessageAsync("this is a test queue message");

            log.LogInformation("Message added to queue.");

            return new OkObjectResult($"Secret2 = {secret.Value}; Message added to queue");
        }
    }
}
