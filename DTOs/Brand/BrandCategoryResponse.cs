using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZooMag.DTOs.Brand
{
    public class BrandCategoryResponse
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string ImagePath { get; set; }
    }
}
