using System.Collections.Generic;

namespace ZooMag.DTOs.Pet
{
    public class PetListItemResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }     
        public int PetCategoryId { get; set; }
        public string Image { get; set; }
        public bool IsActive { get; set; }
        public int QuantityInStock { get; set; }
    }
}