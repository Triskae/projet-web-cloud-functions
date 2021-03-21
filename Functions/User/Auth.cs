using System;
using System.IO;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ProjetWeb.Auth;
using ProjetWeb.Models;
using ProjetWeb.Models.DTO;
using ProjetWeb.Utils;

namespace ProjetWeb.Functions.User
{
    public class Auth
    {
        private readonly IMapper _mapper;
        private readonly IPasswordProvider _passwordProvider;
        private readonly TokenIssuer _tokenIssuer;

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
                return new OkObjectResult(new BaseResponse<string>(_tokenIssuer.IssueTokenForUser(credentials, user)));
            }

            var unauthorizedResponse = new BaseResponse<object>();
            unauthorizedResponse.Errors.Add("Email ou mot de passe incorrect, veuiller réessayer.");
            var unauthorizedResult = new OkObjectResult(unauthorizedResponse)
            {
                StatusCode = StatusCodes.Status401Unauthorized
            };

            return unauthorizedResult;
        }

        [FunctionName("ChangePassword")]
        public async Task<IActionResult> ChangePassword(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)]
            HttpRequest req,
            [CosmosDB("ProjetWeb", "Users", ConnectionStringSetting = "CosmosDB")]
            DocumentClient users,
            ILogger log)
        {
            AuthenticationInfo auth = new AuthenticationInfo(req);

            if (!auth.IsValid)
            {
                return new UnauthorizedResult();
            }

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var data = JsonConvert.DeserializeObject<ChangePasswordDto>(requestBody);

            Models.User foundUser = UserUtils.GetUserFromEmail(users, auth.Email);
            if (foundUser == null)
            {
                return new UnauthorizedResult();
            }

            var isOldPasswordValid =
                _passwordProvider.IsValidPassword(data.OldPassword, foundUser.Salt, foundUser.Password);

            if (!isOldPasswordValid)
            {
                var notFoundResponse = new BaseResponse<object>();
                notFoundResponse.Errors.Add("L'ancien mot de passe n'est pas valide!");
                var notFoundResult = new OkObjectResult(notFoundResponse)
                {
                    StatusCode = StatusCodes.Status401Unauthorized
                };

                return notFoundResult;
            }

            var newPasswordAndSalt = _passwordProvider.GenerateNewSaltedPassword(data.NewPassword);
            var collectionUri = UriFactory.CreateDocumentCollectionUri("ProjetWeb", "Users");
            var query = users.CreateDocumentQuery<Models.User>(collectionUri);

            foundUser.Salt = newPasswordAndSalt.Salt;
            foundUser.Password = newPasswordAndSalt.PasswordHashed;

            await users.UpsertDocumentAsync(collectionUri, foundUser);
            return new OkObjectResult(new BaseResponse<object>());
        }
    }
}