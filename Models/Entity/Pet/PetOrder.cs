using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ZooMag.Models.Entity
{
    public class PetOrder
    {
        [Key]
        public int Id { get; set; }
        public int PetId { get; set; }
        public string Details { get; set; }
        public int PhoneNumber { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
