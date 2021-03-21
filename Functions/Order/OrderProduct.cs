using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ProjetWeb.Auth;
using ProjetWeb.Utils;

namespace ProjetWeb.Functions.Order
{
    public class OrderProduct : AuthorizedServiceBase
    {
        [FunctionName("OrderProduct")]
        public static async Task<IActionResult> RunAsync(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)]
            HttpRequest req,
            [CosmosDB("ProjetWeb", "Users", ConnectionStringSetting = "CosmosDB")]
            DocumentClient users,
            ILogger log)
        {
            string bearer = req.Headers["Authorization"];
            var authHeader = AuthUtils.GetClaims(bearer.Substring(7));
            Models.User foundUser = UserUtils.GetUserFromEmail(users, authHeader["email"].ToString());
            if (foundUser == null)
            {
                return new UnauthorizedResult();
            }

            var collectionUri = UriFactory.CreateDocumentCollectionUri("ProjetWeb", "Users");
            var queryOptions = new FeedOptions {EnableCrossPartitionQuery = true};
            var query = users.CreateDocumentQuery<Models.User>(collectionUri, queryOptions);

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var data = JsonConvert.DeserializeObject<Models.Product>(requestBody);
            var newProduct = new Models.Product
            {
                id = data.id,
                Title = data.Title,
                Description = data.Description,
                Price = data.Price,
                Year = data.Year,
                HorsePower = data.HorsePower,
                Mileage = data.Mileage,
                Fuel = data.Fuel,
                Images = data.Images
            };
            if (foundUser.Orders == null)
            {
                foundUser.Orders = new List<Models.Order>();
                foundUser.Orders.Add(new Models.Order
                {
                    Product = newProduct,
                    IsPayed = true,
                    OrderPlacedDate = DateTime.Now
                });
            }

            foundUser.Orders.Add(new Models.Order
            {
                id = Guid.NewGuid().ToString(),
                Product = newProduct,
                IsPayed = true,
                OrderPlacedDate = DateTime.Now
            });

            await users.UpsertDocumentAsync(collectionUri, foundUser);
            return new OkObjectResult("OK");
        }
    }
}