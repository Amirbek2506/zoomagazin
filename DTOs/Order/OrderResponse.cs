using System;

namespace ZooMag.DTOs.Order
{
    public class OrderResponse
    {
        public int Id { get; set; }
        public string Status { get; set; }
        public string DeliveryAddress { get; set; }
        public DateTime OrderDate { get; set; }
        public double Summa { get; set; }
    }
}