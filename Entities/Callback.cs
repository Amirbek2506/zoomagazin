using System;

namespace ZooMag.Entities
{
    public class Callback
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string FromHour { get; set; }
        public string TillHour { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
