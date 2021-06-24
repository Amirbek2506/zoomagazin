using System.Collections.Generic;
using ZooMag.DTOs.ProductItem;

namespace ZooMag.DTOs.Product
{
    public class ProductDetailsResponse
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string TitleDescription { get; set; }
        public string CategoryName { get; set; }
        public string BrandName { get; set; }
        public int CategoryId { get; set; }
        public int BrandId { get; set; }
        public List<ProductItemDetailsResponse> ProductItems { get; set; }
    }
}