using System.Collections.Generic;

namespace ZooMag.Models.ViewModels.Products
{
    public class FetchProductsRequest
    {
        public List<int> CategoriesId { get; set; }
        public List<int> BrandsId { get; set; }
        public bool IsNew { get; set; } = false;
        public bool IsRecommended { get; set; } = false;
        public int Offset { get; set; } = 0;
        public bool IsTop { get; set; } = false;
        public bool IsSale { get; set; } = false;
        public int MinPrice { get; set; } = 0;
        public int MaxPrice { get; set; } = 0;
        public int Limit { get; set; } = 20;
    }
}