using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using ZooMag.ViewModels;
using ZooMag.Entities;

namespace ZooMag.Models.ViewModels.Products
{
    public class OutPetModel
    {
        public int Id { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string Name { get; set; }
        public string Discription { get; set; }
        public string Color { get; set; }
        public int PetCategoryId { get; set; }
        public string Image { get; set; }
        public bool IsActive { get; set; }
        public string Breed { get; set; }
        public int Age { get; set; }

        public PetCategory Category { get; set; }
        public virtual ICollection<PetImagesModel> Images { get; set; }
    }
}
