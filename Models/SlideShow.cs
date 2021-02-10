using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ZooMag.Models
{
    public class SlideShow
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Link { get; set; }
        public string Image { get; set; }
        public string ImageMobile { get; set; }
        public bool IsActive { get; set; }
        public string Description { get; set; }
        public DateTime DateShow { get; set; }
        public DateTime DateEnd { get; set; }
    }
}
