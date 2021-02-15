using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using projet_web.DatabaseContext;

namespace projet_web
{
    public static class GetAllProducts
    {
        [FunctionName("getAllProducts")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            var options = new DbContextOptionsBuilder<ProjectContext>();
            options.UseCosmos("https://administrator.documents.azure.com:443/", "ChGvETWDYQNic5koOjbfDvOs1dDXSe1VPOwftlWDSvxEIf4DQuyGApjv1QWIMlSCfWrXfy3qDHyd9is0jWSWVg==", "administrator");

            var db = new ProjectContext(options.Options);
            
            log.LogInformation("C# HTTP trigger function processed a request.");

            string name = req.Query["name"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            name = name ?? data?.name;

            string responseMessage = string.IsNullOrEmpty(name)
                ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
                : $"Hello, {name}. This HTTP triggered function executed successfully.";

            return new OkObjectResult(responseMessage);
        }
    }
}
