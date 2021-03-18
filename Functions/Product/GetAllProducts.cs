using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using ProjetWeb.Models;

namespace ProjetWeb.Functions.Product
{
    public static class GetAllProducts
    {
        [FunctionName("GetAllProducts")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)]
            HttpRequest req,
            [CosmosDB("ProjetWeb", "Products", ConnectionStringSetting = "CosmosDB")]
            IEnumerable<Models.Product> products,
            ILogger log
        )
        {
            return new OkObjectResult(new Response<IEnumerable<Models.Product>>(products));
        }
    }
}