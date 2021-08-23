using System.Collections.Generic;

namespace ZooMag.Entities
{
    public class Filter
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public int FilterCategoryId { get; set; }
        public virtual FilterCategory FilterCategory { get; set; }
        public virtual ICollection<CategoryFilter> CategoryFilters { get; set; }
    }
}