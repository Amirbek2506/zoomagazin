using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZooMag.Models.ViewModels.Brands
{
    public class UpdBrandModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public IFormFile Image { get; set; }
    }
}
