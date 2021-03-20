using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace ProjetWeb.Functions.Cart
{
    public static class AddItemToCart
    {
        [FunctionName("AddItemToCart")]
        public static async Task<IActionResult> RunAsync(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)]
            HttpRequest req,
            [CosmosDB("ProjetWeb", "Products", ConnectionStringSetting = "CosmosDB")]
            DocumentClient products,
            [CosmosDB("ProjetWeb", "Users", ConnectionStringSetting = "CosmosDB")]
            DocumentClient users,
            ILogger log)
        {
            
        }
    }
}