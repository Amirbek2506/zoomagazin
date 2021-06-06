using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZooMag.DTOs.Brand
{
    public class CreateBrandRequest
    {
        public string Name { get; set; }
        public IFormFile Image { get; set; }
        public List<int> BrandCategories { get; set; }
    }
}
