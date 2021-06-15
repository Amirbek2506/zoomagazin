using System;
using System.ComponentModel.DataAnnotations;

namespace ZooMag.Entities
{
    public class Chat
    {
        [Key]
        public int Id { get; set; }
        public int FromAnimalId { get; set; }
        public int ToAnimalId { get; set; }
        public string Content { get; set; }
        public bool IsRead { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
