using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ZooMag.Models.Entity;

namespace ZooMag.Models
{
    public class ProductGalery
    {
        [Key]
        public int Id { get; set; }
        public int ProductItemId { get; set; }
        public string Image { get; set; }

        public virtual ProductItem ProductItem { get; set; }
    }
}
