using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ZooMag.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }
        public string UserKey { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal OrderSumm { get; set; }
        public int OrderStatusId { get; set; }
        public string Name { get; set; }
        public int PhoneNumber { get; set; }
        public int AdditionalPhoneNumber1 { get; set; }
        public int AdditionalPhoneNumber2 { get; set; }
        public string City { get; set; }
        public string Email { get; set; }
        public int PaymentMethodId { get; set; }
        public string DeliveryType { get; set; }
        public string DeliveryAddress { get; set; }
        public DateTime DeliveryStartDate { get; set; }
        public DateTime DeliveryEndDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }


        public virtual OrderStatus OrderStatus { get; set; }
        public virtual PaymentMethod PaymentMethod { get; set; }

        public virtual ICollection<OrderItem> OrderItems { get; set; }
    }
}
