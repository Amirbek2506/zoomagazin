using System.Collections.Generic;
using System.Threading.Tasks;
using ZooMag.DTOs;
using ZooMag.DTOs.Brand;
using ZooMag.DTOs.FilterCategory;
using ZooMag.DTOs.SpecificFilter;
using ZooMag.ViewModels;

namespace ZooMag.Services.Interfaces
{
    public interface IBrandsService
    {
        Task<Response> CreateAsync(CreateBrandRequest request);
        Task<Response> UpdateAsync(UpdateBrandRequest request);
        Task<List<BrandResponse>> GetAllAsync();
        Task<List<BrandWithCategoryResponse>> GetAllWithCategoriesAsync();
        Task<BrandResponse> GetByIdAsync(int id);
        Task<List<int>> GetBrandCategoriesAsync(int id);
        Task<Response> DeleteAsync(int id);
        Task<List<AlphabetCharacterWithBrandsResponse>> GetAllAndOrderingByFirstCharacterAsync();
        Task<List<FilterCategoryResponse>> GetBrandFiltersAsync(int brandId);
        Task<List<SpecificFilterResponse>> GetBrandSpecificFiltersAsync(int brandId);
    }
}