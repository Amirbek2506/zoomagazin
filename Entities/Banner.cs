using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZooMag.Entities
{
    public class Banner
    {
        public int Id { get; set; }
        public string ImagePath { get; set; }
        public int PromotionId { get; set; }
    }
}
