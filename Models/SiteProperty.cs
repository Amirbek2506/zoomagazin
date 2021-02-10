using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ZooMag.Models
{
    public class SiteProperty
    {
        [Key]
        public int Id { get; set; }
        public string PropKey { get; set; }
        public string PropValue { get; set; }
    }
}
