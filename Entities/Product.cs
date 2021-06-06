using System;
using System.Collections.Generic;

namespace ZooMag.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string TitleDescription { get; set; }
        public int BrandId { get; set; }
        public DateTime CreateDate { get; set; }
        public bool Removed { get; set; }
        public int CategoryId { get; set; }
        public virtual List<ProductItem> ProductItems { get; set; }
        public virtual Brand Brand { get; set; }
        public virtual Category Category { get; set; }
    }
}