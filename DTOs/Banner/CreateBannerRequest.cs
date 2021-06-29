using Microsoft.AspNetCore.Http;

namespace ZooMag.DTOs.Banner
{
    public class CreateBannerRequest
    {
        public int PromotionId { get; set; }
        public IFormFile Image { get; set; }
    }
}