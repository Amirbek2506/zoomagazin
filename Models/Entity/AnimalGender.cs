using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ZooMag.Models.Entity
{
    public class AnimalGender
    {
        [Key]
        public int Id { get; set; }
        public string TitleEn { get; set; }
        public string TitleRu { get; set; }

        public virtual ICollection<Animal> Animals { get; set; }
    }
}
