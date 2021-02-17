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
        public string TitleEn { get; set; }
        public string TitleRu { get; set; }
        public string Link { get; set; }
        public string Image { get; set; }
        public string ImageMobile { get; set; }
        public bool IsActive { get; set; }
        public string DescriptionEn { get; set; }
        public string DescriptionRu { get; set; }
        public DateTime DateShow { get; set; }
        public DateTime DateEnd { get; set; }
    }
}
