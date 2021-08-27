using System.Collections.Generic;
using ZooMag.DTOs.Brand;

namespace ZooMag.DTOs
{
    public class AlphabetCharacterWithBrandsResponse
    {
        public string Character { get; set; }
        public List<SimpleBrandResponse> Brands { get; set; }
    }
}