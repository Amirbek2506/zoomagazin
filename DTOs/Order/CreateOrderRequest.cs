using System;
using System.Collections.Generic;

namespace ZooMag.DTOs.Order
{
    public class CreateOrderRequest
    {
        public string Email { get; set; }
        public int? PickupPointId { get; set; }
        public DateTime DeliveryTime { get; set; }
        public int? DeliveryFrom { get; set; }
        public int? DeliveryUntil { get; set; }
        public string Comment { get; set; }
        public string DeliveryAddress { get; set; }
    }
}