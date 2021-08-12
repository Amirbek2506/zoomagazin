using Microsoft.AspNetCore.Http;

namespace ZooMag.DTOs.PetGalery
{
    public class CreatePetImageRequest
    {
        public int PetId { get; set; }
        public IFormFile Image { get; set; }
    }
}