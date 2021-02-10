using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ZooMag.Models
{
    public class Size
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }

        public virtual ICollection<ProductSize> ProductSizes { get; set; }
       // public virtual ICollection<Cart> Carts { get; set; }
        //public virtual ICollection<OrderItem> OrderItems { get; set; }
    }
}
