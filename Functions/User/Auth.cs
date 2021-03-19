using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

        public Auth(TokenIssuer tokenIssuer)
        {
            _tokenIssuer = tokenIssuer;
        }


        [FunctionName("Authenticate")]
        public async Task<IActionResult> RunAsync(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)]
            Credentials credentials,
            ILogger log)
        {
            bool authenticated = credentials?.User.Equals("filipe", StringComparison.InvariantCultureIgnoreCase) ??
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