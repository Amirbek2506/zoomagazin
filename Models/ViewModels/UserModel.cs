using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using ZooMag.Entities;

namespace ZooMag.ViewModels
{
    public class UserModel
    {
        public int Id { get; set; }
        public string Image { get; set; }
        public DateTime BirthDay { get; set; }
        public int GenderId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public List<string> Position { get; set; }
        public List<Role> Roles { get; set; }

        public IFormFile file { get; set; }
    }
}
