using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Azure.Security.KeyVault.Secrets;
using Azure.Identity;

namespace API1
{
    public static class test1
    {
        private static readonly string kvSecretUri = "https://keybyh.vault.azure.net/secrets/secret2/651eb25907564cdd91204ae0f3736822";

        [FunctionName("test1")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var client = new SecretClient(new Uri("https://keybyh.vault.azure.net/"), new DefaultAzureCredential());
            KeyVaultSecret secret = await client.GetSecretAsync("secret2");

            // Log the secret
            log.LogInformation($"Secret2 = {secret.Value}");

            return new OkObjectResult($"Secret2 = {secret.Value}");
        }
    }
}
