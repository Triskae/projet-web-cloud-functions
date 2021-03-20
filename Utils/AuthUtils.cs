using System.Collections.Generic;
using JWT.Algorithms;
using JWT.Builder;

namespace ProjetWeb.Utils
{
    public static class AuthUtils
    {
        public static IDictionary<string, object> GetClaims(string bearer)
        {
            return new JwtBuilder()
                .WithAlgorithm(new HMACSHA256Algorithm())
                .WithSecret(Constants.SECRET_KEY)
                .MustVerifySignature()
                .Decode<IDictionary<string, object>>(bearer);
        }
    }
}