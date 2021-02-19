using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ZooMag.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        public int ParentId { get; set; }
        public string TitleEn { get; set; }
        public string TitleRu { get; set; }
        public string Image { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}
