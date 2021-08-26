namespace ZooMag.Entities
{
    public class ProductSpecificFilter
    {
        public int ProductId { get; set; }
        public int SpecificFilterId { get; set; }
        public virtual Product Product { get; set; }
        public virtual SpecificFilter SpecificFilter { get; set; }
    }
}