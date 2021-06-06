using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using ZooMag.DTOs.ProductItem;

namespace ZooMag.DTOs.Product
{
    public class CreateProductRequest
    {
        public string Title { get; set; }
        public string TitleDescription { get; set; }
        public int BrandId { get; set; }
        public int CategoryId { get; set; }
        public List<CreateProductItemRequest> ProductItems { get; set; }
    }
}
