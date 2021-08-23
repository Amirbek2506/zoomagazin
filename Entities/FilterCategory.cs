using System.Collections.Generic;

namespace ZooMag.Entities
{
    public class FilterCategory
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public virtual ICollection<Filter> Filters { get; set; }
    }
}