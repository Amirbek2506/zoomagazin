namespace ZooMag.DTOs.ProductItem
{
    public class WishListProductItemResponse
    {
        public int ProductId { get; set; }
        public int ProductItemId { get; set; }
        public string Title { get; set; }
        public double Price { get; set; }
        public string ImagePath { get; set; }
        public string Measure { get; set; }
    }
}