using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace ProjetWeb.Functions.Product
{
    public class GetProductById
    {
        private CosmosClient cosmosClient;

        public GetProductById(CosmosClient cosmosClient)
        {
            this.cosmosClient = cosmosClient;
        }

        [FunctionName("GetProductById")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)]
            HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var container = cosmosClient.GetContainer("ProjetWeb", "Products");

            try
            {
                var response = container.ReadItemAsync<Models.Product>("id", new PartitionKey("partitionKey"));
                return new OkObjectResult(response.Result.Resource);
            }
            catch (CosmosException cosmosException)
            {
                return new BadRequestObjectResult($"Failed to create item. Cosmos Status Code {cosmosException.StatusCode}, Sub Status Code {cosmosException.SubStatusCode}: {cosmosException.Message}.");
            }
        }
    }
}