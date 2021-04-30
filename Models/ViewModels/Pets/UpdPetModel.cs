using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZooMag.Models.ViewModels.Pets
{
    public class UpdPetModel
    {
        public int Id { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string Name { get; set; }
        public string Discription { get; set; }
        public string Color { get; set; }
        public int PetCategoryId { get; set; }
        public string Breed { get; set; }
        public int Age { get; set; }
    }
}
