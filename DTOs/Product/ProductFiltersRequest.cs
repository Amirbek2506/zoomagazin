using System.Collections.Generic;

namespace ZooMag.DTOs.Product
{
    public class ProductFiltersRequest
    {
        public List<int> BrandsId { get; set; }
        public int CategoryId { get; set; }
        public List<int> FiltersId { get; set; }
        public List<int> SpecificFiltersId { get; set; }
        public double MinPrice { get; set; }
        public double MaxPrice { get; set; }
        public int SortType { get; set; }
    }
}