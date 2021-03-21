using System.Collections.Generic;
using Newtonsoft.Json;

namespace ProjetWeb.Models.DTO
{
    public class UserDto
    {
        [JsonProperty("id")] public string id { get; set; }
        [JsonProperty("firstname")] public string FirstName { get; set; }
        [JsonProperty("lastname")] public string LastName { get; set; }

        [JsonProperty("email")] public string Email { get; set; }
        [JsonProperty("address")] public string Address { get; set; }
        [JsonProperty("postalcode")] public string PostalCode { get; set; }
        [JsonProperty("city")] public string City { get; set; }
        [JsonProperty("password")] public string Password { get; set; }

        [JsonProperty("orders")] public ICollection<Order> Orders { get; set; }
    }
}