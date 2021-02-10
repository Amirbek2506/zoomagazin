using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ZooMag.Models
{
    public class Wishlist
    {
        [Key]
        public int Id { get; set; }
        public string UserKey { get; set; }
        public int ProductId { get; set; }

        public virtual Product Product { get; set; }
    }
}
