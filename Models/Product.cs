using System.Collections.Generic;

namespace ProjetWeb.Models
{
    public class Product
    {
        public string id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public string Year { get; set; }
        
        public int HorsePower { get; set; }
        public int Mileage { get; set; }
        public string Fuel { get; set; }
        public List<string> Images { get; set; }

        public int? OrderId { get; set; }
        public Order? Order { get; set; }
            
        public int? CartId { get; set; }
        public Cart? Cart { get; set; }
    }
}