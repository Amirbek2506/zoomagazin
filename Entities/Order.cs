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
        public int UserId { get; set; }
        public int OrderStatusId { get; set; }
        public string Email { get; set; }
        public string DeliveryAddress { get; set; }
        public DateTime OrderDate { get; set; }
        public int? PickupPointId { get; set; }
        public DateTime? DeliveryTime { get; set; }
        public int? DeliveryFrom { get; set; }
        public int? DeliveryUntil { get; set; }
        public string Comment { get; set; }
        public virtual User User { get; set; }
        public virtual OrderStatus OrderStatus { get; set; }
        public virtual Shop PickupPoint { get; set; }
        public virtual ICollection<OrderProductItem> OrderProductItems { get; set; }
    }
}
