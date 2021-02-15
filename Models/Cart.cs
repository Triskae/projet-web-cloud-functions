using System.Collections.Generic;

namespace ProjetWeb.Models
{
    public class Cart
    {
        public string id { get; set; }
        public ICollection<Product> Products { get; set; }
            
        public int UserId { get; set; }
        public User User { get; set; }
    }
}