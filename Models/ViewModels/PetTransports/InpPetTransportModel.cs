using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ZooMag.Models.ViewModels.PetTransports
{
    public class InpPetTransportModel
    {
        public int PhoneNumber { get; set; }
        public int AnimalTypeId { get; set; }
        public string Comment { get; set; }
        public string FromAddress { get; set; }
        public string ToAddress { get; set; }
        public DateTime Date { get; set; }
    }
}
