using Microsoft.AspNetCore.Http;

namespace zoomagazin.DTOs.Pet
{
    public class UpdatePetRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Age { get; set; }
        public string Breed { get; set; }
        public string Color { get; set; }
        public decimal Price { get; set; }
        public string OriginCountry { get; set; }             
        public int PetCategoryId { get; set; }
        public IFormFile Image { get; set; }
        public bool IsActive { get; set; }
        public int QuantityInStock { get; set; }
        public IFormFile[] Images { get; set; }
        public int UserId { get; set; }
    }
}