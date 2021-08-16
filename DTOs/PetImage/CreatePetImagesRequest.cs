using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace ZooMag.DTOs.PetImage
{
    public class CreatePetImagesRequest
    {
        public int PetId { get; set; }
        public List<IFormFile> Images { get; set; }
    }
}