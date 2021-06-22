using System.Collections.Generic;
using System.Threading.Tasks;
using ZooMag.DTOs.PickupPoint;
using ZooMag.ViewModels;

namespace ZooMag.Services.Interfaces
{
    public interface IPickupPointService
    {
        Task<Response> CreateAsync(CreatePickupPointRequest request);
        Task<List<PickupPointResponse>> GetAllAsync();
        Task<Response> UpdateAsync(UpdatePickupPointRequest request);
        Task<Response> DeleteAsync(int id);
    }
}