using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ZooMag.Models.Entity
{
    public class AnimalType
    {
        [Key]
        public int Id { get; set; }
        public string TitleEn { get; set; }
        public string TitleRu { get; set; }

        public virtual ICollection<Animal> Animals { get; set; }
        public virtual ICollection<PetTransport> PetTransports { get; set; }
    }
}
