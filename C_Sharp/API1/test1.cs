using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Azure.Security.KeyVault.Secrets;
using Azure.Identity;

namespace API1
{
    public static class test1
    {
        private static readonly string kvUrl = "https://keybyh.vault.azure.net";
        private static readonly SecretClient secretClient = new SecretClient(new Uri(kvUrl), new DefaultAzureCredential());

        [FunctionName("test1")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            // Fetch the secret value from Key Vault
            KeyVaultSecret secret = await secretClient.GetSecretAsync("keyvalue2");
            string secretValue = secret.Value;

            log.LogInformation($"secret 2 = this is: {secretValue}");

            string name = req.Query["name"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            name = name ?? data?.name;

            string responseMessage = string.IsNullOrEmpty(name)
                ? $"This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response. Secret Value: {secretValue}"
                : $"Hello, {name}. This HTTP triggered function executed successfully. Secret Value: {secretValue}";

            return new OkObjectResult(responseMessage);
        }
    }
}

