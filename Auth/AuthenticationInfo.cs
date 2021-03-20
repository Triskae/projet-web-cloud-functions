using System;
using System.Collections.Generic;
using System.Linq;
using JWT.Algorithms;
using JWT.Builder;
using Microsoft.AspNetCore.Http;

namespace ProjetWeb.Auth
{
    public class AuthenticationInfo
    {
        public bool IsValid { get; }
        public string Email { get; }
        public string Id { get; }

        public AuthenticationInfo(HttpRequest request)
        {
            if (!request.Headers.ContainsKey("Authorization"))
            {
                IsValid = false;
                return;
            }

            string authorizationHeader = request.Headers["Authorization"];

            if (string.IsNullOrEmpty(authorizationHeader))
            {
                IsValid = false;
                return;
            }

            IDictionary<string, object> claims = null;

            try
            {
                if (authorizationHeader.StartsWith("Bearer"))
                {
                    authorizationHeader = authorizationHeader.Substring(7);
                }

                claims = new JwtBuilder()
                    .WithAlgorithm(new HMACSHA256Algorithm())
                    .WithSecret(Constants.SECRET_KEY)
                    .MustVerifySignature()
                    .Decode<IDictionary<string, object>>(authorizationHeader);
            }
            catch (Exception exception)
            {
                IsValid = false;
                return;
            }

            if (!claims.ContainsKey("email") || !claims.ContainsKey("unique_name"))
            {
                IsValid = false;
                return;
            }

            IsValid = true;
            Email = Convert.ToString(claims["email"]);
            Id = Convert.ToString(claims["unique_name"]);
        }
    }
}