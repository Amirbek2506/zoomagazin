using System.Collections.Generic;
using System.Threading.Tasks;
using ZooMag.Models.Entity;
using ZooMag.Models.ViewModels.Articles;
using ZooMag.Models.ViewModels.PetOrders;
using ZooMag.Models.ViewModels.PetTransports;
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
