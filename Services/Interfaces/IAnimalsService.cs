using System.Collections.Generic;
using System.Threading.Tasks;
using ZooMag.Models.Entity;
using ZooMag.Models.ViewModels.Animals;
using ZooMag.Models.ViewModels.Carts;
using ZooMag.ViewModels;

namespace ZooMag.Services.Interfaces
{
    public interface IAnimalsService
    {
        Task<Response> Create(InpAnimalModel animal,int userid);
        Task<Response> Delete(int id,int userid);
        Task<List<Animal>> GetMyAnimals(int userid);
        Task<List<Animal>> GetAnimals(int typeid,int userid);
        Task<Animal> GetAnimalById(int id);
        Task<Response> UpdateAnimal(UpdAnimalModel model,int userid);
    }
}
