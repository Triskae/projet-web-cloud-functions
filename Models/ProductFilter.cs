namespace ProjetWeb.Models
{
    public class ProductFilter
    {
        public string Keyword { get; set; }
        public int LowerPriceLimit { get; set; }
        public int UpperPriceLimit { get; set; }
        public int LowerHorsepowerLimit { get; set; }
        public int UpperHorsepowerLimit { get; set; }
    }
}