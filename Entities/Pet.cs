using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ZooMag.Models;

namespace ZooMag.Entities
{
    public class Pet
    {
        [Key]
        public int Id { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Color { get; set; }
        public int PetCategoryId { get; set; }
        public string Image { get; set; }
        public bool IsActive { get; set; }
        public string Breed { get; set; }
        public int Age { get; set; }


        public virtual ICollection<PetGalery> PetGaleries { get; set; }
    }
}
