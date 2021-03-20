namespace ProjetWeb.Models.DTO
{
    public class ProductResponse<T>
    {
        public T Data { get; set; }
        public int? MinimumPrice { get; set; }
        public int? MaximumPrice { get; set; }
        
        public ProductResponse(T data, int? minimumPrice = null, int? maximumPrice = null)
        {
            Data = data;
            MinimumPrice = minimumPrice;
            MaximumPrice = maximumPrice;
        }
    }
}