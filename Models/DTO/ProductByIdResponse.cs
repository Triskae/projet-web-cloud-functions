using System.Collections.Generic;

namespace ProjetWeb.Models.DTO
{
    public class ProductByIdResponse<T>
    {
        public T Data { get; set; }
        public List<Product> OtherProducts { get; set; }
        
        public ProductByIdResponse(T data, List<Product> otherProducts)
        {
            Data = data;
            OtherProducts = otherProducts;
        }
    }
}