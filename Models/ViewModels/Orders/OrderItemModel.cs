using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using ZooMag.Models.ViewModels.Products;

namespace ZooMag.Models.ViewModels.Orders
{
    public class OrderItemModel
    {
        public int Id { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
        public int Quantity { get; set; }

        public OutProductModel Product { get; set; }
        public Size Size { get; set; }
    }
}
