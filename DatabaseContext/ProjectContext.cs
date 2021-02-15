using System;
using System.Collections.Generic;
using System.Net.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace projet_web.DatabaseContext
{
    public class ProjectContext : DbContext
    {
        public ProjectContext(DbContextOptions options) : base(options)
        {
        }

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

        public class Order
        {
            public int OrderId { get; set; }
            public DateTime OrderPlacedDate { get; set; }
            public bool IsPayed { get; set; }

            public ICollection<Product> Products { get; set; }

            public int UserId { get; set; }
            public User USer { get; set; }
        }

        public class User
        {
            public int UserId { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Adress { get; set; }
            public string PostalCode { get; set; }
            public string City { get; set; }
            public string Avatar { get; set; }

            public ICollection<Order> Orders { get; set; }

            public int CartId { get; set; }
            public Cart Cart { get; set; }
        }

        public class Cart
        {
            public ICollection<Product> Products { get; set; }
            
            public int UserId { get; set; }
            public User User { get; set; }
        }
    }
}