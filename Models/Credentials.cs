using Newtonsoft.Json;

namespace ProjetWeb.Models
{
    public class Credentials
    {
        [JsonProperty("user")]
        public string Email { get; set; }
        [JsonProperty("password")]
        public string Password { get; set; }
    }
}