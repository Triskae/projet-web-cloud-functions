using System.Collections.Generic;
using Newtonsoft.Json;

namespace ProjetWeb.Models
{
    public class Product
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        
        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }
        
        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }
        
        [JsonProperty(PropertyName = "price")]
        public int Price { get; set; }
        
        [JsonProperty(PropertyName = "year")]
        public string Year { get; set; }
        
        [JsonProperty(PropertyName = "horsepower")]
        public int HorsePower { get; set; }
        
        [JsonProperty(PropertyName = "mileage")]
        public int Mileage { get; set; }
        
        [JsonProperty(PropertyName = "fuel")]
        public string Fuel { get; set; }
        
        [JsonProperty(PropertyName = "images")]
        public List<string> Images { get; set; }
    }
}