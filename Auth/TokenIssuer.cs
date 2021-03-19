using System.Collections.Generic;
using JWT;
using JWT.Algorithms;
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
        
        public string IssueTokenForUser(Credentials credentials)
        {
            Dictionary<string, object> claims = new Dictionary<string, object>
            {
                { "username", credentials.User },
                { "role", "admin"}
            };

            string token = _jwtEncoder.Encode(claims, Constants.SECRET_KEY);

            return token;
        }
    }
}