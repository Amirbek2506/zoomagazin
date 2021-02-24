using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ZooMag.Models.ViewModels.Orders
{
    public class InpOrderModel
    {
        [Required]
        [StringLength(9, MinimumLength = 9)]
        public string PhoneNumber { get; set; }
        public int PayMethodId { get; set; }
        public int DeliveryType { get; set; }
        public string DeliveryAddress { get; set; }
    }
}
