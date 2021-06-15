using System.ComponentModel.DataAnnotations;

namespace ZooMag.Entities
{
    public class PetCategory
    {
        [Key]
        public int Id { get; set; }
        public int ParentId { get; set; }
        public string Title { get; set; }
        public string Image { get; set; }
    }
}
