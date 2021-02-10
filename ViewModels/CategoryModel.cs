using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ZooMag.ViewModels
{
    public class CategoryModel
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public List<CategoryModel> SubCategories { get; set; }

    }
}
