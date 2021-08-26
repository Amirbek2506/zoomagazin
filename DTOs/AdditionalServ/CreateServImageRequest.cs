using Microsoft.AspNetCore.Http;

namespace ZooMag.DTOs.AdditionalServ
{
    public class CreateServImageRequest
    {
        public int AdditionalServId { get; set; }
        public IFormFile Image { get; set; }
        public bool IsBannerImage { get; set; }
    }
}