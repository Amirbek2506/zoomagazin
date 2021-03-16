﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ZooMag.Models.Entity;

namespace ZooMag.Models
{
    public class OrderStatus
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<PetTransport> PetTransports { get; set; }
    }
}
