using System.Threading.Tasks;
using ZooMag.DTOs.Description;
using ZooMag.ViewModels;

namespace ZooMag.Services.Interfaces
{
    public interface IDescriptionService
    {
        Task<Response> CreateAsync(CreateDescriptionRequest request);
        Task<Response> UpdateAsync(UpdateDescriptionRequest request);
        Task<Response> DeleteAsync(int id);
    }
}