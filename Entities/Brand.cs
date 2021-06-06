using System.Collections.Generic;

namespace ZooMag.Entities
{
    public class Brand
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImagePath { get; set; }
        public virtual ICollection<BrandCategory> BrandCategories { get; set; }
    }
}