namespace ZooMag.DTOs.ProductItem
{
    public class MostPopularProductItemResponse
    {
        public int Id { get; set; }
        public string Measure { get; set; }
        public double? Price { get; set; }
        public double SellingPrice { get; set; }
        public double Discount { get; set; }
    }
}