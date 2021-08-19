using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace ZooMag.DTOs.AdditionalServ
{
    public class CreateAdditionalServRequest
    {
        public string ServName { get; set; }
        public string ContentText { get; set; }
        public bool IsActive { get; set; }
        public List<IFormFile> BannerImages { get; set; }
        public List<IFormFile> OtherImages { get; set; }
    }
}