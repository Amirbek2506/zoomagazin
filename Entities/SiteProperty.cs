using System.ComponentModel.DataAnnotations;

namespace ZooMag.Entities
{
    public class SiteProperty
    {
        [Key]
        public int Id { get; set; }
        public string PropKey { get; set; }
        public string PropValueEn { get; set; }
        public string PropValueRu { get; set; }
    }
}
