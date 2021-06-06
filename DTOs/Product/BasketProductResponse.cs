namespace ZooMag.DTOs.Product
{
    public class BasketProductResponse
    {
        public int ProductItemId { get; set; }
        public int ProductId { get; set; }
        public string Title { get; set; }
        public string TitleDescription { get; set; }
        public string ImagePath { get; set; }
        public double Price { get; set; }
        public int Count { get; set; }
        public double Percent { get; set; }
        public double SellingPrice { get; set; }
        public double Benefit { get; set; }
        public string Measure { get; set; }
        public string VendorCode { get; set; }
        public double Summa { get; set; }
    }
}