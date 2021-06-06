using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZooMag.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public bool IsDelivery { get; set; }
        public int? DeliveryTypeId { get; set; }
        public string UserId { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public DateTime OrderDate { get; set; }
    }
}
