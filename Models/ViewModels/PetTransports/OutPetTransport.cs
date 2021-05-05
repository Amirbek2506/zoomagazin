using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZooMag.Models.Entity;
using ZooMag.ViewModels;

namespace ZooMag.Models.ViewModels.PetTransports
{
    public class OutPetTransport
    {
        public int Id { get; set; }
        public int PhoneNumber { get; set; }
        public string Comment { get; set; }
        public string FromAddress { get; set; }
        public string ToAddress { get; set; }
        public DateTime Date { get; set; }
        public DateTime CreatedAt { get; set; }

        public virtual OrderStatus OrderStatus { get; set; }
        public virtual UserModel User { get; set; }
    }
}
