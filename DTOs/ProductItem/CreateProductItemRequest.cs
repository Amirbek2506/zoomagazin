using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using ZooMag.DTOs.Description;

namespace ZooMag.DTOs.ProductItem
{
    public class CreateProductItemRequest
    {
        public string VendorCode { get; set; }
        public string Measure { get; set; }
        public double Price { get; set; }
        public double Percent { get; set; }
        public int? ProductId { get; set; }
        public List<string> Descriptions { get; set; }
        public List<IFormFile> Images { get; set; }
    }
}
