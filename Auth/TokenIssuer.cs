using System;
using System.Collections.Generic;
using JWT;
using JWT.Algorithms;
using JWT.Builder;
using JWT.Serializers;
using ProjetWeb.Models;

namespace ProjetWeb.Auth
{
    public class TokenIssuer
    {
        private readonly IJwtAlgorithm _algorithm;
        private readonly IJsonSerializer _serializer;
        private readonly IBase64UrlEncoder _base64Encoder;
        private readonly IJwtEncoder _jwtEncoder;

        public TokenIssuer()
        {
            _algorithm = new HMACSHA256Algorithm();
            _serializer = new JsonNetSerializer();
            _base64Encoder = new JwtBase64UrlEncoder();
            _jwtEncoder = new JwtEncoder(_algorithm, _serializer, _base64Encoder);
        }

        public string IssueTokenForUser(Credentials credentials, User user)
        {
            Dictionary<string, object> claims = new Dictionary<string, object>
            {
                {"email", credentials.Email},
                {"unique_name", user.id},
                {"iat", DateTimeOffset.Now.ToUnixTimeSeconds().ToString()},
                {"jti", Guid.NewGuid().ToString()}
            };

            var token = new JwtBuilder()
                .WithAlgorithm(new HMACSHA256Algorithm())
                .WithSecret(Constants.SECRET_KEY)
                .AddClaims(claims)
                .Encode();

            return token;
        }
    }
}