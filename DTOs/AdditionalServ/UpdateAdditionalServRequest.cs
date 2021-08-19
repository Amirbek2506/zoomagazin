using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace ZooMag.DTOs.AdditionalServ
{
    public class UpdateAdditionalServRequest
    {
        public int Id { get; set; }
        public string ServName { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsActive { get; set; }
        public string ContentText { get; set; }
        public List<IFormFile> BannerImages { get; set; }
        public List<IFormFile> OtherImages { get; set; }
    }
}