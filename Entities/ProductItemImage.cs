namespace ZooMag.Entities
{
    public class ProductItemImage
    {
        public int Id { get; set; }
        public string ImagePath { get; set; }
        public int ProductItemId { get; set; }
        public virtual ProductItem ProductItem { get; set; }
    }
}
