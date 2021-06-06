using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZooMag.DTOs.Product
{
    public class UpdateProductRequest
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string TitleDescription { get; set; }
        public int BrandId { get; set; }
        public int CategoryId { get; set; }
    }
}
