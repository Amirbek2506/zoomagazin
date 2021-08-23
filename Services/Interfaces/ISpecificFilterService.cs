using System.Collections.Generic;
using System.Threading.Tasks;
using ZooMag.DTOs.SpecificFilter;
using ZooMag.ViewModels;

namespace ZooMag.Services.Interfaces
{
    public interface ISpecificFilterService
    {
        Task<Response> CreateAsync(CreateSpecificFilterRequest request);
        Task<List<SpecificFilterResponse>> GetAllAsync();
    }
}