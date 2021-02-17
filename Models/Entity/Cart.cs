using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ZooMag.Models
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
