using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ZooMag.Models.ViewModels.Carts;

namespace ZooMag.Models.ViewModels.Orders
{
    public class InpOrderModel
    {
        [Required]
        public int PhoneNumber { get; set; }
        public int PayMethodId { get; set; }
        public int DeliveryType { get; set; }
        public string DeliveryAddress { get; set; }

        public List<InpCartModel> carts { get; set; }
    }
}
