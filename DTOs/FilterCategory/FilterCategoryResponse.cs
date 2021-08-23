using System.Collections.Generic;
using ZooMag.DTOs.Filter;

namespace ZooMag.DTOs.FilterCategory
{
    public class FilterCategoryResponse
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public List<FilterResponse> Filters { get; set; }
    }
}