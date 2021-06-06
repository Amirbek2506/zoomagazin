using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZooMag.Entities
{
    public class Review
    {
        public int Id { get; set; }
        public string Comment { get; set; }
        public DateTime CommentDate { get; set; }
        public int ProductItemId { get; set; }
        public int Rating { get; set; }
        public bool Removed { get; set; }
        public virtual ProductItem ProductItem { get; set; }
    }
}
