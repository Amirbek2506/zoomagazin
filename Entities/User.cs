using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace ZooMag.Entities
{
    public class User : IdentityUser<int>
    {
        public static object Identity { get; internal set; }
        public string Image { get; set; }
        public DateTime BirthDay { get; set; }
        public int GenderId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public virtual Gender Gender { get; set; }
        public virtual ICollection<Comment> Reviews { get; set; }
        public virtual ICollection<Animal> Animals { get; set; }
        public virtual ICollection<Pet> Pets { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<PetTransport> PetTransports { get; set; }
        public virtual ICollection<BoxOrder> BoxOrders { get; set; }
    }
}
