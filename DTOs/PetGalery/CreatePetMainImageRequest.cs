using Microsoft.AspNetCore.Http;

namespace ZooMag.DTOs.PetGalery
{
    public class CreatePetMainImageRequest
    {
        public int PetId { get; set; }
        public IFormFile Image { get; set; }
    }
}