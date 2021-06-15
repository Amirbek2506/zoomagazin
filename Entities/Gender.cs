using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ZooMag.Entities
{
    public class Gender
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}
