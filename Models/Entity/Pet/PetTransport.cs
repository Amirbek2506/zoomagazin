using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ZooMag.Models.Entity
{
    public class PetTransport
    {
        [Key]
        public int Id { get; set; }
        public int OrderStatusId { get; set; }
        public int PhoneNumber { get; set; }
        public int AnimalType { get; set; }
        public int UserId { get; set; }
        public string Comment { get; set; }
        public string FromAddress { get; set; }
        public string ToAddress { get; set; }
        public DateTime Date { get; set; }
        public DateTime CreatedAt { get; set; }

        public virtual OrderStatus OrderStatus { get; set; }
        public virtual User User { get; set; }
    }
}