using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZooMag.Entities
{
    public class OrderProductItem
    {
        public int OrderId { get; set; }
        public int ProductItemId { get; set; }
        public virtual ProductItem ProductItem { get; set; }
        public virtual Order Order { get; set; }
        public int Count { get; set; }
    }
}
