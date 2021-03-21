namespace ProjetWeb.Models.DTO
{
    public class ProductResponse<T>
    {
        public T Data { get; set; }
        public int? MinimumPrice { get; set; }
        public int? MaximumPrice { get; set; }
        public int? MinimumHorsepower { get; set; }
        public int? MaximumHorsepower { get; set; }
        
        public ProductResponse(T data, int? minimumPrice = null, int? maximumPrice = null, int? minimumHorsepower = null, int? maximumHorsepower = null)
        {
            Data = data;
            MinimumPrice = minimumPrice;
            MaximumPrice = maximumPrice;
            MinimumHorsepower = minimumHorsepower;
            MaximumHorsepower = maximumHorsepower;
        }
    }
}