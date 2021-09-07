using System.Collections.Generic;
using ZooMag.DTOs.PetImage;

namespace ZooMag.DTOs.Pet
{
    public class PetListItemResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Age { get; set; }
        public string Breed { get; set; }
        public string Color { get; set; }
        public decimal Price { get; set; }
        public int? MainImageId { get; set; }        
        public string OriginCountry { get; set; }             
        public int PetCategoryId { get; set; }
        public bool IsActive { get; set; }
        public int QuantityInStock { get; set; }
        public List<GetPetImageResponse> Images { get; set; }
    }
}