using System;
using System.IO;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ProjetWeb.Auth;
using ProjetWeb.Models.DTO;

namespace ProjetWeb.Functions.User
{
    public class RegisterUser
    {
        private readonly IPasswordProvider _passwordProvider;
        private readonly IMapper _mapper;

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
                    Address = userReq.Address,
                    City = userReq.City,
                    PostalCode = userReq.PostalCode,
                    Salt = saltAndHash.Salt,
                    Password = saltAndHash.PasswordHashed,
                };
                await users.AddAsync(userToRegister);
                return new OkObjectResult(new BaseResponse<UserDto>(_mapper.Map<UserDto>(userToRegister)));
            }
            catch (Exception ex)
            {
                var conflictResponse = new BaseResponse<object>();
                conflictResponse.Errors.Add("Cet adresse email est déjà utilisée par un autre compte, veuillez en utiliser une autre.");
                var unauthorizedResult = new OkObjectResult(conflictResponse)
                {
                    StatusCode = StatusCodes.Status409Conflict
                };

                return unauthorizedResult;
            }
        }
    }
}