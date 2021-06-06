using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ZooMag.Models.Entity
{
    public class ProductItem
    {
        [Key]
        public int Id { get; set; }
        public int VendorCode { get; set; }
        public int ProductId { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal OriginalPrice { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal SellingPrice { get; set; }
        public int Quantity { get; set; }
        public string Weight { get; set; }
        public string Measure { get; set; }
        public int Discount { get; set; }

        public bool IsActive { get; set; }

        public virtual Product Product { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; }
        public virtual ICollection<ProductGalery> ProductGaleries { get; set; }
    }
}
