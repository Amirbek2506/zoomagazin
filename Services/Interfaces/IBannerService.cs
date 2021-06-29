using System.Collections.Generic;
using System.Threading.Tasks;
using ZooMag.DTOs;
using ZooMag.DTOs.Banner;
using ZooMag.ViewModels;

namespace ZooMag.Services.Interfaces
{
    public interface IBannerService
    {
        Task<Response> CreateAsync(CreateBannerRequest request);
        Task<Response> UpdateAsync(UpdateBannerRequest request);
        Task<Response> DeleteAsync(int id);
        Task<List<BannerResponse>> GetAllAsync(PagedRequest request);
    }
}