
using System.Collections.Generic;

namespace ZooMag.Entities
{
    public class PickupPoint
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}