using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZooMag.ViewModels;

namespace ZooMag.Models.ViewModels.Products
{
    public class FirstProductModel
    {
        public int Id { get; set; }
        public string NameEn { get; set; }
        public string NameRu { get; set; }
        public string DiscriptionEn { get; set; }
        public string DiscriptionRu { get; set; }
        public string ShortDiscriptionEn { get; set; }
        public string ShortDiscriptionRu { get; set; }
        public string ColorEn { get; set; }
        public string ColorRu { get; set; }
        public double Weight { get; set; }
        public bool IsNew { get; set; }
        public decimal OriginalPrice { get; set; }
        public decimal SellingPrice { get; set; }
        public bool IsSale { get; set; }
        public DateTime SaleStartDate { get; set; }
        public DateTime SaleEndDate { get; set; }
        public int Quantity { get; set; }
        public string Image { get; set; }
        public int BrandId { get; set; }
        public bool IsTop { get; set; }
        public bool IsRecommended { get; set; }

        public Category category { get; set; }
        public Measure measure { get; set; }

        public List<ProductImagesModel> Images { get; set; }
        public List<SizeModel> Sizes { get; set; }
    }
}
