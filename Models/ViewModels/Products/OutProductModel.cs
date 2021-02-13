﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZooMag.ViewModels;

namespace ZooMag.Models.ViewModels.Products
{
    public class OutProductModel
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

        public List<ProductImagesModel> ProductImages { get; set; }
        public List<SizeModel> Sizes { get; set; }
    }
}
