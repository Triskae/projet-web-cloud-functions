using System;
using System.Collections.Generic;
using JWT.Algorithms;
using JWT.Builder;
using Microsoft.AspNetCore.Http;

namespace ProjetWeb.Auth
{
    public class AuthenticationInfo
    {
        public bool IsValid { get; }
        public string Username { get; }
        public string Role { get; }

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

            if (!claims.ContainsKey("username"))
            {
                IsValid = false;

                return;
            }

            IsValid = true;
            Username = Convert.ToString(claims["username"]);
            Role = Convert.ToString(claims["role"]);
        }
    }
}