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
using ProjetWeb.Models.DTO;
using ProjetWeb.Utils;

namespace ProjetWeb.Functions.User
{
    public class UpdateUser : AuthorizedServiceBase
    {
        [FunctionName("UpdateUser")]
        public async Task<IActionResult> RunAsync(
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
            var query = users.CreateDocumentQuery<Models.User>(collectionUri);

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var data = JsonConvert.DeserializeObject<Models.DTO.CreateOrUpdateUserDto>(requestBody);

            foundUser.FirstName = data.FirstName;
            foundUser.LastName = data.LastName;
            foundUser.Address = data.Address;
            foundUser.PostalCode = data.PostalCode;
            foundUser.City = data.City;

            await users.UpsertDocumentAsync(collectionUri, foundUser);
            return new OkObjectResult(new BaseResponse<Models.User>(foundUser));
        }
    }
}