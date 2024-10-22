﻿using System.ComponentModel.DataAnnotations;

namespace ZooMag.Entities
{
    public class PetImage
    {
        [Key]
        public int Id { get; set; }
        public int PetId { get; set; }
        public string ImageUrl { get; set; }
        public virtual Pet Pet { get; set; }
    }
}