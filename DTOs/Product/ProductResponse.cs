using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ZooMag.DTOs.ProductItem;

namespace ZooMag.DTOs.Product
{
    public class ProductResponse
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string TitleDescription { get; set; }
        public string ImagePath { get; set; }
        public int BrandId { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string BrandName { get; set; }
        public List<ProductItemResponse> ProductItems { get; set; }
    }
}