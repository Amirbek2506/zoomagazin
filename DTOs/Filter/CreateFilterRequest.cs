using System.Collections.Generic;

namespace ZooMag.DTOs.Filter
{
    public class CreateFilterRequest
    {
        public string Text { get; set; }
        public int FilterCategoryId { get; set; }
        public List<int> CategoryIds { get; set; }
    }
}