using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ZooMag.Models.ViewModels.Hostel
{
    public class BoxOrderModel
    {
        public int BoxTypeId { get; set; }
        public string Comment { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int PhoneNumber { get; set; }
    }
}
