using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace ZooMag.DTOs.PetGalery
{
    public class CreatePetGaleryRequest
    {
        public int PetId { get; set; }
        public List<IFormFile> Images { get; set; }
    }
}