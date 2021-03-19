using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using ProjetWeb.Models;

namespace ProjetWeb.Functions.Product
{
    public static class GetProductById
    {
        [FunctionName("GetProductById")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)]
            HttpRequest req,
            [CosmosDB("ProjetWeb", "Products", ConnectionStringSetting = "CosmosDB", Id = "{Query.id}", PartitionKey = "{Query.id}")]
            Models.Product product,
            ILogger log)
        {
            return new OkObjectResult(new Response<Models.Product>(product));
        }
    }
}