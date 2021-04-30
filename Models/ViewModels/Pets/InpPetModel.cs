using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ZooMag.Models;

namespace ZooMag.ViewModels
{
    public class InpPetModel
    {
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string Name { get; set; }
        public string Discription { get; set; }
        public string Color { get; set; }
        public int PetCategoryId { get; set; }
        public string Breed { get; set; }
        public int Age { get; set; }

        public IFormFile[] Images { get; set; }
    }
}
