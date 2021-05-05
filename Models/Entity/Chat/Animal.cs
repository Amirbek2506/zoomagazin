using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ZooMag.Models.Entity
{
    public class Animal
    {
        [Key]
        public int Id { get; set; }
        public  string Name { get; set; }
        public int AnimalType { get; set; }
        public int UserId { get; set; }
        public string Image { get; set; }
        public string Breed { get; set; }
        public string Color { get; set; }
        public int AnimalGender { get; set; }
        public int Age { get; set; }
        public DateTime CreateAt { get; set; }

        public virtual User User { get; set; }
    }
}
