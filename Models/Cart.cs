using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public DateTime CreatedAt { get; set; }

        public virtual Product Product { get; set; }
       //public virtual Size Size { get; set; }
    }
}
