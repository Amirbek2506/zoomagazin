using Microsoft.AspNetCore.Http;

namespace ZooMag.DTOs.AdditionalServ
{
    public class CreateServImageRequest
    {
        public IFormFile Image { get; set; }
        public bool IsBanner { get; set; }
    }
}