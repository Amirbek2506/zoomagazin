using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZooMag.Models.Entity;
using ZooMag.ViewModels;

namespace ZooMag.Models.ViewModels.Hostel
{
    public class OutBoxOrderModel
    {
        public int Id { get; set; }
        public int PhoneNumber { get; set; }
        public string Comment { get; set; }
        public string Status { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public DateTime CreatedAt { get; set; }

        public virtual UserModel User { get; set; }
        public virtual BoxType BoxType { get; set; }
        public virtual Box Box { get; set; }
    }
}
