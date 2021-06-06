using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using ZooMag.Models.Entity;

namespace ZooMag.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ShortDescription { get; set; }
        public string Color { get; set; }
        public int CategoryId { get; set; }
        public int BrandId { get; set; }
        public string Image { get; set; }
        public bool IsNew { get; set; }
        public bool IsSale { get; set; }
        public bool IsTop { get; set; }
        public bool IsRecommended { get; set; }
        public int Discount { get; set; }

        public bool IsActive { get; set; }
        public int PromotionId { get; set; }

        public virtual ICollection<ProductItem> ProductItems { get; set; }
        public virtual ICollection<Wishlist> Wishlists { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }
    }
}
