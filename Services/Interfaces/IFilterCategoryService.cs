using System.Collections.Generic;
using System.Threading.Tasks;
using ZooMag.DTOs.FilterCategory;
using ZooMag.ViewModels;

namespace ZooMag.Services.Interfaces
{
    public interface IFilterCategoryService
    {
        Task<List<FilterCategoryResponse>> GetAllAsync();
        Task<Response> CreateAsync(CreateFilterCategoryRequest request);
    }
}