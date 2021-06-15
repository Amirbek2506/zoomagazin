using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace ZooMag.DTOs.Promotion
{
    public class CreatePromotionRequest
    {
        public IFormFile Image { get; set; }
        public string Title { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public double Percent { get; set; }
        public List<int> ProductItems { get; set; }
    }
}