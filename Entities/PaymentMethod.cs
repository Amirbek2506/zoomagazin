using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ZooMag.Entities
{
    public class PaymentMethod
    {
        [Key]
        public int Id { get; set; }
        public string MethodName { get; set; }

        public virtual ICollection<Order> Orders { get; set; }

    }
}
