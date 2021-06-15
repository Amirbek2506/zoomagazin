using System.Collections.Generic;
using System.Threading.Tasks;
using ZooMag.Entities;
using ZooMag.Models.ViewModels.PetOrders;
using ZooMag.ViewModels;

namespace ZooMag.Services.Interfaces
{
    public interface IPetOrdersService
    {
        Task<Response> Create(PetOrderModel model);
        Task<Response> Delete(int id);
        Task<List<PetOrder>> Get();
    }
}
