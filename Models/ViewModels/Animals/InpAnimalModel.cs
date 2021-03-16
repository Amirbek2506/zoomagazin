using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZooMag.Models.ViewModels.Animals
{
    public class InpAnimalModel
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public int AnimalGenderId { get; set; }
        public int AnimalTypeId { get; set; }
        public string Breed { get; set; }
        public string Color { get; set; }

        public IFormFile Image { get; set; }
    }
}
