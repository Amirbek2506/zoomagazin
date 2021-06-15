using System.Collections.Generic;
using System.Threading.Tasks;
using ZooMag.Models.ViewModels.Articles;
using ZooMag.Models.ViewModels.PetTransports;
using ZooMag.ViewModels;

namespace ZooMag.Services.Interfaces
{
    public interface IPetTransportsService
    {
        Task<Response> Create(InpPetTransportModel model, int userid);
        Task<Response> Delete(int id);
        Task<List<OutPetTransport>> Get();
        Task<OutPetTransport> GetById(int id);
        Task<Response> ChangeStatus(int id, int statusid);
    }
}
