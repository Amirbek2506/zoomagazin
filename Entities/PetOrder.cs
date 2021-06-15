using System;
using System.ComponentModel.DataAnnotations;

namespace ZooMag.Entities
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
