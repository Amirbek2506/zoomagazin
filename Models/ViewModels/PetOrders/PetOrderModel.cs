using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ZooMag.Models.ViewModels.PetOrders
{
    public class PetOrderModel
    {
        public string Details { get; set; }
        [StringLength(9, MinimumLength = 9)]
        public int PhoneNumber { get; set; }
    }
}
