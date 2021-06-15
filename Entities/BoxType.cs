using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ZooMag.Entities
{
    public class BoxType
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public double Price { get; set; }

        public virtual ICollection<BoxOrder> BoxOrders { get; set; }
        public virtual ICollection<Box> Boxes { get; set; }
    }
}
