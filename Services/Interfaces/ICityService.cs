using System.Collections.Generic;
using System.Threading.Tasks;
using ZooMag.DTOs;
using ZooMag.DTOs.City;
using ZooMag.ViewModels;

namespace ZooMag.Services.Interfaces
{
    public interface ICityService
    {
        Task<Response> CreateAsync(CreateCityRequest request);
        Task<GenericResponse<List<CityResponse>>> GetAllAsync(PagedRequest request);
        Task<Response> UpdateAsync(UpdateCityRequest request);
        Task<Response> DeleteAsync(int id);
    }
}