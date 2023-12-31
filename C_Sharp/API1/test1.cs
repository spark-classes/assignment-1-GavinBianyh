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
    public static class Test1
    {
         private static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        
        private static readonly string kvUri = "https://keybyh.vault.azure.net/";

        [FunctionName("test1")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            try
            {
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

                string messageToBeSent = "this is a test queue message";
                await queueClient.SendMessageAsync(Base64Encode(messageToBeSent));

                log.LogInformation("Message added to queue.");

                // Return a successful response
                return new OkObjectResult($"Secret2 = {secret.Value}; Message added to queue");
            }
            catch (Exception e)
            {
                // Log the error and handle it accordingly
                log.LogError($"Exception thrown: {e.Message}");

                // Determine the status code to return
                var statusCode = e is UnauthorizedAccessException ? StatusCodes.Status403Forbidden : StatusCodes.Status500InternalServerError;

                // Return an error response
                return new ObjectResult(new { error = e.Message, stackTrace = e.StackTrace })
                {
                    StatusCode = statusCode
                };
            }
        }

    }
}
