using System;

namespace ZooMag.DTOs.Promotion
{
    public class PromotionResponse
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string ImagePath { get; set; }
    }
}