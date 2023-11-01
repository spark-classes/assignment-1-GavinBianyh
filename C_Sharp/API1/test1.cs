using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Azure.KeyVault;

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

            var azureServiceTokenProvider = new AzureServiceTokenProvider();
            var keyVaultClient = new KeyVaultClient(
                new KeyVaultClient.AuthenticationCallback(azureServiceTokenProvider.KeyVaultTokenCallback));

            var secret = await keyVaultClient.GetSecretAsync(kvSecretUri)
                .ConfigureAwait(false);

            // Log the secret
            log.LogInformation($"Secret2 = {secret.Value}");

            return new OkObjectResult($"Secret2 = {secret.Value}");
        }
    }
}
