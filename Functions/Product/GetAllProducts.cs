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
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)]
            HttpRequest req,
            [CosmosDB("ProjetWeb", "Products", ConnectionStringSetting = "CosmosDB")]
            DocumentClient client,
            ILogger log
        )
        {
            log.LogInformation("GetAllProducts");

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
                             (filter.UpperPriceLimit == 0 || p.Price <= filter.UpperPriceLimit) &&
                             (filter.LowerHorsepowerLimit == 0 || p.HorsePower >= filter.LowerHorsepowerLimit) &&
                             (filter.UpperHorsepowerLimit == 0 || p.HorsePower <= filter.UpperHorsepowerLimit)
                    )
                    .AsDocumentQuery();
            }
            else
            {
                query = client.CreateDocumentQuery<Models.Product>(collectionUri).AsDocumentQuery();
            }

            var everyProducts = req.Method == HttpMethods.Post
                ? client
                    .CreateDocumentQuery<Models.Product>(collectionUri)
                    .AsDocumentQuery()
                    .ExecuteNextAsync<Models.Product>().Result.ToList()
                : query
                    .ExecuteNextAsync<Models.Product>()
                    .Result
                    .ToList();

            var minimumPrice = everyProducts.Min(x => x.Price);
            var maximumPrice = everyProducts.Max(x => x.Price);
            var minimumHorsepower = everyProducts.Min(x => x.HorsePower);
            var maximumHorsepower = everyProducts.Max(x => x.HorsePower);

            return new OkObjectResult(
                new BaseResponse<ProductResponse<List<Models.Product>>>(new ProductResponse<List<Models.Product>>(
                    query.ExecuteNextAsync<Models.Product>().Result.ToList(), minimumPrice,
                    maximumPrice, minimumHorsepower, maximumHorsepower)));
        }
    }
}