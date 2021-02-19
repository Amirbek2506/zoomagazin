using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZooMag.Models.ViewModels.Categories
{
    public class InpCategoryModel
    {
        public int ParentId { get; set; }
        public string TitleEn { get; set; }
        public string TitleRu { get; set; }
        public IFormFile Image { get; set; }
    }
}
