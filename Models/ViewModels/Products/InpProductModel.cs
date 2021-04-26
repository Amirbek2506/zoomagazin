using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ZooMag.Models;

namespace ZooMag.ViewModels
{
    public class InpProductModel
    {
        public string NameEn { get; set; }
        public string NameRu { get; set; }
        public string DiscriptionEn { get; set; }
        public string DiscriptionRu { get; set; }
        public string ShortDiscriptionEn { get; set; }
        public string ShortDiscriptionRu { get; set; }
        public string ColorEn { get; set; }
        public string ColorRu { get; set; }
        public int CategoryId { get; set; }
        public int MeasureId { get; set; }
        public double Weight { get; set; }
        public bool IsNew { get; set; }
        public decimal OriginalPrice { get; set; }
        public decimal SellingPrice { get; set; }
        public bool IsSale { get; set; }
        public DateTime? SaleStartDate { get; set; }
        public DateTime? SaleEndDate { get; set; }
        public int Quantity { get; set; }
        public int BrandId { get; set; }
        public bool IsTop { get; set; }
        public bool IsRecommended { get; set; }

        public List<string> Sizes { get; set; }
        public IFormFile[] Images { get; set; }
    }
}
