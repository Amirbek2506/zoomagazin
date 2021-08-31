using System.Collections.Generic;
using System.Threading.Tasks;
using ZooMag.DTOs.Shop;
using ZooMag.ViewModels;

namespace ZooMag.Services.Interfaces
{
    public interface IShopService
    {
        Task<Response> CreateAsync(CreateShopRequest request);
        Task<List<PickupPointResponse>> GetAllPickupPointsAsync();
        Task<List<ShopResponse>> GetAllAsync();
        Task<Response> UpdateAsync(UpdateShopRequest request);
        Task<Response> DeleteAsync(int id);
        Task<List<PickupPointResponse>> GetAllPickupPointsByCityIdAsync(int cityId);
        Task<ShopResponse> GetByIdAsync(int shopId);
    }
}