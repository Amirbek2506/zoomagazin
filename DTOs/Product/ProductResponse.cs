using System.Collections.Generic;
using ZooMag.DTOs.ProductItem;

namespace ZooMag.DTOs.Product
{
    public class ProductResponse
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string TitleDescription { get; set; }
        public string ImagePath { get; set; }
        public List<ProductItemResponse> ProductItems { get; set; }
    }
}