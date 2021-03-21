using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using ProjetWeb.Auth;
using ProjetWeb.Models.DTO;
using ProjetWeb.Utils;

namespace ProjetWeb.Functions.User
{
    public class GetMe : AuthorizedServiceBase
    {
        private readonly IMapper _mapper;

        public GetMe(IMapper mapper)
        {
            _mapper = mapper;
        }

        [FunctionName("Me")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)]
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
                var notFoundResponse = new BaseResponse<object>();
                notFoundResponse.Errors.Add("Utilisateur non trouvé.");
                var notFoundResult = new OkObjectResult(notFoundResponse)
                {
                    StatusCode = StatusCodes.Status401Unauthorized
                };

                return notFoundResult;
            }

            return new OkObjectResult(new BaseResponse<UserDto>(_mapper.Map<UserDto>(foundUser)));
        }
    }
}