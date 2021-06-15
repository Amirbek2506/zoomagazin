using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using ZooMag.Entities;

namespace ZooMag.Models.ViewModels.Orders
{
    public class OutOrderModel
    {
        public int Id { get; set; }
        public string UserKey { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal OrderSumm { get; set; }
        public int PhoneNumber { get; set; }
        public string DeliveryType { get; set; }
        public string DeliveryAddress { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }


        public OrderStatus OrderStatus { get; set; }
        public PaymentMethod PaymentMethod { get; set; }

        public List<OrderItemModel> OrderItems { get; set; }
    }
}
