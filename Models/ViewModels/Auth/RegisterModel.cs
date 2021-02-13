using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace ZooMag.ViewModels
{
    public class RegisterModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        
        [Required]
        [StringLength(60, MinimumLength = 3)]
        public string Password { get; set; }
        [Required]
        [StringLength(60, MinimumLength = 3)]
        public string ConfirmPassword { get; set; }
    }
}