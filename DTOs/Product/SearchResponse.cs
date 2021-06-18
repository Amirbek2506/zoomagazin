using System.Collections.Generic;
using ZooMag.DTOs.Category;

namespace ZooMag.DTOs.Product
{
    public class SearchResponse
    {
        public List<SearchCategoryResponse> Categories { get; set; }
        public int ProductsCount { get; set; }
        public List<SearchProductResponse> Products { get; set; }
    }
}