using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZooMag.Entities
{
    public class Description
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int ProductItemId { get; set; }
        public virtual ProductItem ProductItem { get; set; }
    }
}
