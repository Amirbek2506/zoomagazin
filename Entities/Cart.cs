using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZooMag.Entities
{
    public class Cart
    {
        [Key]
        public int Id { get; set; }
        public string UserKey { get; set; }
        public int ProductId { get; set; }
        public int SizeId { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public DateTime CreatedAt { get; set; }

        public virtual Product Product { get; set; }
        //public virtual Size Size { get; set; }
    }
}
