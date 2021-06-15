using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ZooMag.Entities
{
    public class OrderStatus
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<PetTransport> PetTransports { get; set; }
    }
}
