using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using JWT.Algorithms;
using JWT.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ProjetWeb.Auth;

namespace ProjetWeb.Functions.Cart
{
    public class AddItemToCart : AuthorizedServiceBase
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
            string bearer = req.Headers["Authorization"];
            
            var authHeader = new JwtBuilder()
                .WithAlgorithm(new HMACSHA256Algorithm())
                .WithSecret(Constants.SECRET_KEY)
                .MustVerifySignature()
                .Decode<IDictionary<string, object>>(bearer.Substring(7));
            Console.WriteLine(authHeader);
            return new OkObjectResult("OK");
        }
    }
}