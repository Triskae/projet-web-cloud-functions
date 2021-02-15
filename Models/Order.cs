using System;
using System.Collections.Generic;

namespace ProjetWeb.Models
{
    public class Order
    {
        public string id { get; set; }
        public DateTime OrderPlacedDate { get; set; }
        public bool IsPayed { get; set; }

        public ICollection<Product> Products { get; set; }

        public int UserId { get; set; }
        public User USer { get; set; }
    }
}