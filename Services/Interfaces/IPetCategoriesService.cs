using System.Collections.Generic;
using System.Threading.Tasks;
using ZooMag.Entities;
using ZooMag.Models.ViewModels.Categories;
using ZooMag.Models.ViewModels.PetCategories;
using ZooMag.ViewModels;

namespace ZooMag.Services.Interfaces
{
    public interface IPetCategoriesService
    {
        Task<Response> Create(InpPetCategoryModel model);
        PetCategory FetchById(int id);
        Task<List<PetCategory>> Fetch();
        Task<List<OutPetCategoryModel>> FetchWithSubcategories();
        Task<Response> Update(UpdPetCategoryModel model);
        Task<Response> Delete(int id);
    }
}