namespace ZooMag.Entities
{
    public class Wishlist
    {
        public string UserId { get; set; }
        public int ProductItemId { get; set; }
        public virtual User User { get; set; }
        public virtual ProductItem ProductItem { get; set; }
    }
}