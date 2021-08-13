using Microsoft.AspNetCore.Http;
using ZooMag.DTOs.PetGalery;

namespace ZooMag.DTOs.Pet
{
    public class GetPetResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Age { get; set; }
        public string Breed { get; set; }
        public string Color { get; set; }
        public decimal Price { get; set; }
        public string OriginCountry { get; set; }             
        public int PetCategoryId { get; set; }
        public string Image { get; set; }
        public bool IsActive { get; set; }
        public int QuantityInStock { get; set; }
        public GetPetGaleryResponse Images { get; set; }
        public int UserId { get; set; }
    }
}