using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ProjetWeb.Auth;
using ProjetWeb.Models;
using ProjetWeb.Models.DTO;

namespace ProjetWeb.Functions.Product
{
    public class GetAllProducts : AuthorizedServiceBase
    {
        [FunctionName("GetAllProducts")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)]
            HttpRequest req,
            [CosmosDB("ProjetWeb", "Products", ConnectionStringSetting = "CosmosDB")]
            DocumentClient client,
            ILogger log
        )
        {
            var collectionUri = UriFactory.CreateDocumentCollectionUri("ProjetWeb", "Products");
            IDocumentQuery<Models.Product> query;

            if (req.Method == HttpMethods.Post)
            {
                var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                var filter = JsonConvert.DeserializeObject<ProductFilter>(requestBody);

                var queryOptions = new FeedOptions {EnableCrossPartitionQuery = true};
                query = client.CreateDocumentQuery<Models.Product>(collectionUri, queryOptions)
                    .Where(
                        p => filter == null ||
                             (string.IsNullOrEmpty(filter.Keyword) || p.Title.ToLower()
                                  .Contains(string.IsNullOrEmpty(filter.Keyword)
                                      ? string.Empty
                                      : filter.Keyword.ToLower()) ||
                              p.Description.ToLower().Contains(string.IsNullOrEmpty(filter.Keyword)
                                  ? string.Empty
                                  : filter.Keyword.ToLower())) &&
                             (filter.LowerPriceLimit == 0 || p.Price >= filter.LowerPriceLimit) &&
                             (filter.UpperPriceLimit == 0 || p.Price <= filter.LowerPriceLimit)
                    )
                    .AsDocumentQuery();
            }
            else
            {
                query = client.CreateDocumentQuery<Models.Product>(collectionUri).AsDocumentQuery();
            }

            return new OkObjectResult(
                new BaseResponse<List<Models.Product>>(query.ExecuteNextAsync<Models.Product>().Result.ToList()));
        }
    }
}