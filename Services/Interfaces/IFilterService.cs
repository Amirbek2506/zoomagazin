using System.Collections.Generic;
using System.Threading.Tasks;
using ZooMag.DTOs.Filter;
using ZooMag.ViewModels;

namespace ZooMag.Services.Interfaces
{
    public interface IFilterService
    {
        Task<Response> CreateAsync(CreateFilterRequest request);
        Task<List<FilterResponse>> GetAllAsync();
        Task<List<SelectOptionFilterResponse>> GetFilterForSelectOptionAsync();
    }
}