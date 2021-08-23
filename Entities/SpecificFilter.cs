using System.Collections.Generic;

namespace ZooMag.Entities
{
    public class SpecificFilter
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public virtual ICollection<ProductSpecificFilter> ProductSpecificFilters { get; set; }
    }
}