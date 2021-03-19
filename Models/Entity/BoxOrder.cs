using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ZooMag.Models.Entity
{
    public class BoxOrder
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public int BoxTypeId { get; set; }
        public string Comment { get; set; }
        public int PhoneNumber { get; set; }
        public string Status { get; set; }
        public int BoxId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public DateTime CreatedAt { get; set; }

        public virtual User User { get; set; }
        public virtual BoxType BoxType { get; set; }
    }
}
