using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ZooMag.Models.Entity
{
    public class BoxType
    {
        [Key]
        public int Id { get; set; }
        public string TitleEn { get; set; }
        public string TitleRu { get; set; }
        public double Price { get; set; }

        public virtual ICollection<BoxOrder> BoxOrders { get; set; }
        public virtual ICollection<Box> Boxes { get; set; }
    }
}
