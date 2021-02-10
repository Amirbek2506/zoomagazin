using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ZooMag.Models
{
    public class ProductGalery
    {
        [Key]
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string Image { get; set; }

        public virtual Product Product { get; set; }
    }
}
