using System;
using System.ComponentModel.DataAnnotations;

namespace ZooMag.Entities
{
    public class Article
    {
        [Key]
        public int Id { get; set; }
        public string Image { get; set; }
        public string Title { get; set; }
        public string ShortDescription { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
