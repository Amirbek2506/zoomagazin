using System.Collections.Generic;
using ZooMag.DTOs.ProductItem;

namespace ZooMag.DTOs.Product
{
    public class ProductDetailsResponse
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string TitleDescription { get; set; }
        public List<ProductItemDetailsResponse> ProductItems { get; set; }
    }
}