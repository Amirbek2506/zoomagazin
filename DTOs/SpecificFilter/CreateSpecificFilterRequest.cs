using System.Collections.Generic;

namespace ZooMag.DTOs.SpecificFilter
{
    public class CreateSpecificFilterRequest
    {
        public string Text { get; set; }
        public List<int> ProductIds { get; set; }
    }
}