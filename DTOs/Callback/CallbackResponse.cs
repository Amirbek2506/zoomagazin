using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZooMag.DTOs.Callback
{
    public class CallbackResponse
    {
        public int Id { get; set; }
        public string PhoneNumber { get; set; }
        public string FromHour { get; set; }
        public string TillHour { get; set; }
        public string Name { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
