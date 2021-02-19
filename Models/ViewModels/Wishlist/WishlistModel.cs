using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZooMag.Models.ViewModels.Products;

namespace ZooMag.Models.ViewModels.Wishlist
{
    public class WishlistModel
    {
        public int Id { get; set; }
        public OutProductModel Product { get; set; }
    }
}
