using System.ComponentModel.DataAnnotations;

namespace ZooMag.Entities
{
    public class PetGalery
    {
        [Key]
        public int Id { get; set; }
        public int PetId { get; set; }
        public string Image { get; set; }
        public virtual Pet Pet { get; set; }
    }
}