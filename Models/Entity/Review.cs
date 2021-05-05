using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ZooMag.Models
{
    public class Review
    {
        [Key]
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string UserName { get; set; }
        public double Value { get; set; }
        public string Text { get; set; }
        public DateTime CreatedAt { get; set; }

        public virtual Product Product { get; set; }
    }
}
