using System;
using System.Collections.Generic;

namespace ZooMag.Entities
{
    public class AdditionalServ
    {
        public int Id { get; set; }
        public string ServName { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsActive { get; set; }
        public string ContentText { get; set; }
        public virtual IEnumerable<ServImages> ServImages { get; set; }
    }
}