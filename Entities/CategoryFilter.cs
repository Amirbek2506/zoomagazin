namespace ZooMag.Entities
{
    public class CategoryFilter
    {
        public int CategoryId { get; set; }
        public int FilterId { get; set; }
        public virtual Category Category { get; set; }
        public virtual Filter Filter { get; set; }
    }
}