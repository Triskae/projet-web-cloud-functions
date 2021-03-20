using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using ProjetWeb.Auth;
using ProjetWeb.Models;
using ProjetWeb.Models.DTO;

namespace ProjetWeb.Functions.Product
{
    public class GetProductById : AuthorizedServiceBase
    {
        [FunctionName("GetProductById")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)]
            HttpRequest req,
            [CosmosDB("ProjetWeb", "Products", ConnectionStringSetting = "CosmosDB", Id = "{Query.id}", PartitionKey = "{Query.id}")]
            Models.Product product,
            ILogger log)
        {
            return new OkObjectResult(new BaseResponse<Models.Product>(product));
        }
    }
}