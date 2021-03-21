using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using ProjetWeb.Auth;
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
            [CosmosDB("ProjetWeb", "Products", ConnectionStringSetting = "CosmosDB")]
            DocumentClient client,
            ILogger log)
        {
            if (product != null)
            {
                var collectionUri = UriFactory.CreateDocumentCollectionUri("ProjetWeb", "Products");
                var otherProducts = client.CreateDocumentQuery<Models.Product>(collectionUri)
                    .AsDocumentQuery()
                    .ExecuteNextAsync<Models.Product>()
                    .Result
                    .Where(p => p.id != product.id)
                    .OrderBy(p => Guid.NewGuid())
                    .Take(3)
                    .ToList();
            
                return new OkObjectResult(
                    new BaseResponse<ProductByIdResponse<Models.Product>>(new ProductByIdResponse<Models.Product>(product, otherProducts)));
            }

            var notFoundResponse = new BaseResponse<object>();
            notFoundResponse.Errors.Add("Aucun produit n'a été trouvé.");
            var notFoundResult = new OkObjectResult(notFoundResponse)
            {
                StatusCode = StatusCodes.Status404NotFound
            };

            return notFoundResult;
        }
    }
}