using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ZooMag.Models.ViewModels.Chats
{
    public class InpChatModel
    {
        [Required]
        public int FromAnimalId { get; set; }
        [Required]
        public int ToAnimalId { get; set; }
        [Required]
        public string Content { get; set; }
    }
}
