using System.ComponentModel.DataAnnotations;

namespace ZooMag.Entities
{
    public class ProductGalery
    {
        [Key]
        public int Id { get; set; }
        public int ProductItemId { get; set; }
        public string Image { get; set; }

        public virtual ProductItem ProductItem { get; set; }
    }
}
