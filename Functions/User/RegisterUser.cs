using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using ProjetWeb.Auth;
using ProjetWeb.Models.DTO;

namespace ProjetWeb.Functions.User
{
    public class RegisterUser
    {
        private readonly IMapper _mapper;
        private readonly IPasswordProvider _passwordProvider;

        public RegisterUser(IPasswordProvider passwordProvider, IMapper mapper)
        {
            _passwordProvider = passwordProvider;
            _mapper = mapper;
        }

        [FunctionName("RegisterUser")]
        public async Task<IActionResult> RunAsync(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)]
            UserDto userReq,
            [CosmosDB("ProjetWeb", "Users", ConnectionStringSetting = "CosmosDB")]
            IAsyncCollector<Models.User> users,
            ILogger log)
        {
            try
            {
                var saltAndHash = _passwordProvider.GenerateNewSaltedPassword(userReq.Password);
                var userToRegister = new Models.User
                {
                    Email = userReq.Email,
                    FirstName = userReq.FirstName,
                    LastName = userReq.LastName,
                    Address = string.Empty,
                    City = string.Empty,
                    PostalCode = string.Empty,
                    Salt = saltAndHash.Salt,
                    Password = saltAndHash.PasswordHashed,
                };
                await users.AddAsync(userToRegister);
                return new OkObjectResult(new BaseResponse<UserDto>(_mapper.Map<UserDto>(userToRegister)));
            }
            catch (Exception ex)
            {
                var conflictResponse = new BaseResponse<object>();
                conflictResponse.Errors.Add(
                    "Cet adresse email est déjà utilisée par un autre compte, veuillez en utiliser une autre.");
                var conflictResult = new OkObjectResult(conflictResponse)
                {
                    StatusCode = StatusCodes.Status409Conflict
                };

                return conflictResult;
            }
        }
    }
}