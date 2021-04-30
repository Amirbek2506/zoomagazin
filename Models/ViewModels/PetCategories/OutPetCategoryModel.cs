using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ZooMag.ViewModels
{
    public class OutPetCategoryModel
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Image { get; set; }
        public List<OutPetCategoryModel> SubCategories { get; set; }

    }
}
