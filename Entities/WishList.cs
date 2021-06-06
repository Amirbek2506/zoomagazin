using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZooMag.Models;

namespace ZooMag.Entities
{
    public class WishList
    {
        public string UserId { get; set; }
        public int ProductItemId { get; set; }
        public virtual  User User { get; set; }
        public virtual ProductItem ProductItem { get; set; }
    }
}
