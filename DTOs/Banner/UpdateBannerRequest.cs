using Microsoft.AspNetCore.Http;

namespace ZooMag.DTOs.Banner
{
    public class UpdateBannerRequest
    {
        public int Id { get; set; }
        public IFormFile Image { get; set; }
    }
}