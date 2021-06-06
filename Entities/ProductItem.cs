using System.Collections.Generic;

namespace ZooMag.Entities
{
    public class ProductItem
    {
        public int Id { get; set; }
        public string VendorCode { get; set; }
        public string Measure { get; set; }
        public double Price { get; set; }
        public double Percent { get; set; }
        public int ProductId { get; set; }
        public bool Removed { get; set; }
        public int? PromotionId { get; set; }
        public virtual List<Review> Reviews { get; set; }
        public virtual Product Product { get; set; }
        public virtual List<Description> Descriptions { get; set; }
        public virtual List<ProductItemImage> ProductItemImages { get; set; }
    }
}