using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZooMag.Entities
{
    public class OrderProduct
    {
        public int OrderId { get; set; }
        public int ProductItemId { get; set; }
        public int Count { get; set; }
    }
}
