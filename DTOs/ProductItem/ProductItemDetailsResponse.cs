using System.Collections.Generic;
using ZooMag.DTOs.Description;

namespace ZooMag.DTOs.ProductItem
{
    public class ProductItemDetailsResponse
    {
        public int Id { get; set; }
        public string Measure { get; set; }
        public double SellingPrice { get; set; }
        public double Price { get; set; }
        public string VendorCode { get; set; }
        public double Rating { get; set; }
        public int CommentsCount { get; set; }
        public List<string> ImagesPath { get; set; }
        public List<DescriptionDetailsResponse> Descriptions { get; set; }
    }
}