using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ZooMag.Models;
using ZooMag.Models.Entity;
using ZooMag.Models.ViewModels.ProductItems;

namespace ZooMag.ViewModels
{
    public class InpProductModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string ShortDescription { get; set; }
        public string Color { get; set; }
        public int CategoryId { get; set; }
        public int BrandId { get; set; }
        public bool IsNew { get; set; }
        public bool IsSale { get; set; }
        public bool IsTop { get; set; }
        public bool IsRecommended { get; set; }
        public int Discount { get; set; }


        public ICollection<ProductItemModel> ProductItems { get; set; }
    }
}
