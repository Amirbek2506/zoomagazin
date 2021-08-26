using Microsoft.AspNetCore.Http;

namespace ZooMag.DTOs.PetImage
{
    public class CreatePetImageRequest
    {
        public int PetId { get; set; }
        public IFormFile Image { get; set; }
    }
}