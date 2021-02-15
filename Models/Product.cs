using System.Collections.Generic;

namespace ProjetWeb.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public string Category { get; set; }
        public List<string> Images { get; set; }

        public int? OrderId { get; set; }
        public Order? Order { get; set; }
            
        public int? CartId { get; set; }
        public Cart? Cart { get; set; }
    }
}