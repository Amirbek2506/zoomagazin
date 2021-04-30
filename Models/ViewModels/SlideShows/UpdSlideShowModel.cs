using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZooMag.Models.ViewModels.SlideShows
{
    public class UpdSlideShowModel
    {
        public int Id { get; set; }
        public string Category { get; set; }
        public IFormFile Image { get; set; }
        public IFormFile ImageMobile { get; set; }
    }
}
