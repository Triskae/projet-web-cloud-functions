using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace ProjetWeb.Functions.Product
{
    public static class GetAllProducts
    {
        [FunctionName("getAllProducts")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            [CosmosDB("ProjetWeb", "Products", ConnectionStringSetting ="CosmosDB")]
            IEnumerable<Models.Product> products,
            ILogger log)
        {
            return new OkObjectResult(products);
        }
    }
}
