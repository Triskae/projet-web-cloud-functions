using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using ProjetWeb.Auth;
using ProjetWeb.Models;
using ProjetWeb.Utils;

namespace ProjetWeb.Functions.User
{
    public class Auth
    {
        private readonly TokenIssuer _tokenIssuer;
        private readonly IPasswordProvider _passwordProvider;
        private readonly IMapper _mapper;

        public Auth(TokenIssuer tokenIssuer, IPasswordProvider passwordProvider, IMapper mapper)
        {
            _tokenIssuer = tokenIssuer;
            _passwordProvider = passwordProvider;
            _mapper = mapper;
        }


        [FunctionName("Authenticate")]
        public async Task<IActionResult> RunAsync(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)]
            Credentials credentials,
            [CosmosDB("ProjetWeb", "Users", ConnectionStringSetting = "CosmosDB")]
            DocumentClient client,
            ILogger log)
        {
            Models.User user = null;
            try
            {
                user = UserUtils.GetUserFromEmail(client, credentials.Email);
            }
            catch (Exception e)
            {
                return new NotFoundResult();
            }

            if (user != null && _passwordProvider.IsValidPassword(credentials.Password, user.Salt,
                user.Password))
            {
                return new OkObjectResult(_tokenIssuer.IssueTokenForUser(credentials, user));
            }

            return new UnauthorizedResult();
        }

        [FunctionName("ChangePassword")]
        public async Task<IActionResult> ChangePassword(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)]
            HttpRequest req,
            ILogger log)
        {
            AuthenticationInfo auth = new AuthenticationInfo(req);

            if (!auth.IsValid)
            {
                return new UnauthorizedResult();
            }

            string newPassword = await req.ReadAsStringAsync();

            return new OkObjectResult($"{auth.Email} changed password to {newPassword}");
        }
    }
}