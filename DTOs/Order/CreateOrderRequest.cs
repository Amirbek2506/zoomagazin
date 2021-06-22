using System;
using System.Collections.Generic;

namespace ZooMag.DTOs.Order
{
    public class CreateOrderRequest
    {
        public int? DeliveryTypeId { get; set; }
        public string PhoneNumber { get; set; }
        public string AdditionalPhoneNumber { get; set; }
        public string SecondAdditionalPhoneNumber { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public int? PickupPointId { get; set; }
        public DateTime DeliveryTime { get; set; }
        public string Comment { get; set; }
        public int PaymentMethodId { get; set; }
        public List<int> ProductItemIds { get; set; }
    }
}