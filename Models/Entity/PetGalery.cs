using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ZooMag.Models
{
    public class PetGalery
    {
        [Key]
        public int Id { get; set; }
        public int PetId { get; set; }
        public string Image { get; set; }

        public virtual Pet Pet { get; set; }
    }
}
