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
    public class ProductModel
    {
        
        public int Id { get; set; }
        public string Name { get; set; }
        public string Discription { get; set; } 
        public string ShortDiscription { get; set; }
        public int CategoryId { get; set; }
        public int MeasureId { get; set; }
        public string Color { get; set; }
        public double Weight { get; set; }
        public bool IsNew { get; set; }
        public decimal OriginalPrice { get; set; }
        public decimal SellingPrice { get; set; }
        public bool IsSale { get; set; }
        public DateTime SaleStartDate { get; set; }
        public DateTime SaleEndDate { get; set; }
        public int Quantity { get; set; }
        public string Image { get; set; }



        public List<string> InpSizes { get; set; }
        public List<ProductImagesModel> ProductImages { get; set; }
        public List<SizeModel> OutSizes { get; set; }

        public IFormFile[] Images { get; set; }
    }
}
