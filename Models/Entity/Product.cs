using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ZooMag.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal OriginalPrice { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal SellingPrice { get; set; }
        public bool IsSale { get; set; }
        public DateTime SaleStartDate { get; set; }
        public DateTime SaleEndDate { get; set; }
        public int Quantity { get; set; }
        public string NameEn { get; set; }
        public string NameRu { get; set; }
        public string DiscriptionEn { get; set; }
        public string DiscriptionRu { get; set; }
        public string ShortDiscriptionEn { get; set; }
        public string ShortDiscriptionRu { get; set; }
        public string ColorEn { get; set; }
        public string ColorRu { get; set; }
        public int MeasureId { get; set; }
        public int CategoryId { get; set; }
        public string Image { get; set; }
        public bool IsNew { get; set; }
        public string PreOrder { get; set; }
        public string Link { get; set; }
        public bool IsActive { get; set; }
        public double Weight { get; set; }


        public virtual Category Category { get; set; }
        public virtual ICollection<Cart> Carts { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; }
        public virtual ICollection<ProductGalery> ProductGaleries { get; set; }
        public virtual ICollection<Wishlist> Wishlists { get; set; }
        public virtual ICollection<ProductSize> ProductSizes { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }
    }
}
