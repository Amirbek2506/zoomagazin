namespace ZooMag.DTOs.Product
{
    public class SearchProductResponse
    {
        public int ProductId { get; set; }
        public string Title { get; set; }
        public double Price { get; set; }
        public string ImagePath { get; set; }
        public string VendorCode { get; set; }
        public int ProductItemId { get; set; }
    }
}