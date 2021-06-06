using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZooMag.DTOs.ProductItem
{
    public class UpdateProductItemRequest
    {
        public int Id { get; set; }
        public string VendorCode { get; set; }
        public string Measure { get; set; }
        public double Price { get; set; }
        public double Percent { get; set; }
        public List<IFormFile> Images { get; set; }
    }
}
