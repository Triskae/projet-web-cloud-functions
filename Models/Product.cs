using System.Collections.Generic;
using Newtonsoft.Json;

namespace ProjetWeb.Models
{
    public class Product
    {
        [JsonProperty("id")]
        public string id { get; set; }
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("price")]
        public int Price { get; set; }
        [JsonProperty("year")]
        public string Year { get; set; }
        [JsonProperty("horsepower")]
        public int HorsePower { get; set; }
        [JsonProperty("mileage")]
        public int Mileage { get; set; }
        [JsonProperty("fuel")]
        public string Fuel { get; set; }
        [JsonProperty("images")]
        public List<string> Images { get; set; }
    }
}