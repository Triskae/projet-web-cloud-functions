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
using ProjetWeb.Models;

namespace ProjetWeb.Functions.User
{
    public class Auth
    {
        private readonly TokenIssuer _tokenIssuer;
        private readonly IPasswordProvider _passwordProvider;

        public Auth(TokenIssuer tokenIssuer, IPasswordProvider passwordProvider)
        {
            _tokenIssuer = tokenIssuer;
            _passwordProvider = passwordProvider;
        }


        [FunctionName("Authenticate")]
        public async Task<IActionResult> RunAsync(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)]
            Credentials credentials,
            [CosmosDB("ProjetWeb", "Users", ConnectionStringSetting = "CosmosDB")]
            DocumentClient client,
            ILogger log)
        {
            var collectionUri = UriFactory.CreateDocumentCollectionUri("ProjetWeb", "Users");
            var query = client.CreateDocumentQuery<Models.User>(collectionUri)
                .Where(u => u.Email == credentials.Email)
                .AsDocumentQuery();
            var foundUser = query.ExecuteNextAsync<Models.User>().Result.ToList().First();
            
            
            
            Console.WriteLine(_passwordProvider.GenerateNewSaltedPassword(credentials?.Password));
            bool authenticated = credentials?.Email.Equals("filipe", StringComparison.InvariantCultureIgnoreCase) ??
                                 false;

            if (!authenticated)
            {
                return new UnauthorizedResult();
            }

            return new OkObjectResult(_tokenIssuer.IssueTokenForUser(credentials));
        }

        [FunctionName("ChangePassword")]
        public async Task<IActionResult> ChangePassword(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "changepassword")]
            HttpRequest req,
            ILogger log)
        {
            AuthenticationInfo auth = new AuthenticationInfo(req);

            if (!auth.IsValid)
            {
                return new UnauthorizedResult();
            }

            string newPassword = await req.ReadAsStringAsync();

            return new OkObjectResult($"{auth.Username} changed password to {newPassword}");
        }
    }
}