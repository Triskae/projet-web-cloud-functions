using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ProjetWeb.Auth;

namespace ProjetWeb.Functions.Product
{
    public class CreateProduct : AuthorizedServiceBase
    {
        [FunctionName("CreateProduct")]
        public async Task<IActionResult> RunAsync(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)]
            HttpRequest req, [CosmosDB("ProjetWeb", "Products", ConnectionStringSetting = "CosmosDB")]
            IAsyncCollector<Models.Product> products, ILogger log)
        {
            try
            {
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                var data = JsonConvert.DeserializeObject<Models.Product>(requestBody);
                var newProduct = new Models.Product
                {
                    Title = data.Title,
                    Description = data.Description,
                    Price = data.Price,
                    Year = data.Year,
                    HorsePower = data.HorsePower,
                    Mileage = data.Mileage,
                    Fuel = data.Fuel,
                    Images = data.Images
                };
                await products.AddAsync(newProduct);
                return new OkObjectResult(newProduct);
            }
            catch (Exception ex)
            {
                log.LogError($"Couldn't insert item. Exception thrown: {ex.Message}");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}