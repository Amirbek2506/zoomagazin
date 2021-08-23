using System.Collections.Generic;

namespace ZooMag.Entities
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImagePath { get; set; }
        public int? ParentCategoryId { get; set; }
        public virtual ICollection<CategoryFilter> CategoryFilters { get; set; }
        public virtual ICollection<BrandCategory> BrandCategories { get; set; }
        public virtual Category ParentCategory { get; set; }
        public virtual ICollection<Category> Categories { get; set; }
    }
}