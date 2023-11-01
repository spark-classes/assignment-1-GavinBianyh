namespace API2
{
    public class QueueProcessor
    {
        string storageConnectionString = Environment.GetEnvironmentVariable("AZURE_STORAGE_CONNECTION_STRING");


        [FunctionName("QueueProcessor")]
        public void Run([QueueTrigger("api1queue", Connection = "newsetting")]string myQueueItem, ILogger log)
        {
            //log.LogInformation($"Third secret's value: {secret3.Value}");
            
            log.LogInformation($"C# Queue trigger function processed: {myQueueItem}");
        }
    }
}
