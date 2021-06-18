using System.Collections.Generic;

namespace ZooMag.DTOs.Product
{
    public class ProductFiltersRequest
    {
        public List<int> BrandsId { get; set; }
        public List<int> CategoriesId { get; set; }
        public double MinPrice { get; set; }
        public double MaxPrice { get; set; }
        public string SortType { get; set; }
    }
}