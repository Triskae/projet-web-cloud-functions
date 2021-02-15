using System.Collections.Generic;
using ProjetWeb.Models;

namespace ProjetWeb
{
    public class User
    {
        public string id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
        public string Avatar { get; set; }

        public ICollection<Order> Orders { get; set; }

        public int CartId { get; set; }
        public Cart Cart { get; set; }
    }
}