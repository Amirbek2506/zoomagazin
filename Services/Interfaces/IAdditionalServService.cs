using System.Collections.Generic;
using System.Threading.Tasks;
using ZooMag.DTOs.AdditionalServ;
using ZooMag.ViewModels;

namespace ZooMag.Services.Interfaces
{
    public interface IAdditionalServService
    {
        Task<Response> CreateAdditionalService(CreateAdditionalServRequest request);
        Task<List<int>> CreateServImages(List<CreateServImageRequest> request, int addiotionalServId);
        Task<List<GetAdditionalServResponse>> GetAllAdditionalServ();
        Task<GetAdditionalServResponse> GetAdditionalServ(int additionalServId);
        Task<List<GetServImageResponse>> GetServImages(int addiotionalServId);
        Task<Response> UpdateAdditionalServ(UpdateAdditionalServRequest request);
        Task<Response> DeleteAdditionalServ(int additionalServId);
        Task<Response> DeleteServImage(int servImageId);

    }
}