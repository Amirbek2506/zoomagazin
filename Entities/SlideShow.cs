using System.ComponentModel.DataAnnotations;

namespace ZooMag.Entities
{
    public class SlideShow
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Link { get; set; }
        public string Category { get; set; }
        public string Image { get; set; }
        public string ImageMobile { get; set; }
    }
}
