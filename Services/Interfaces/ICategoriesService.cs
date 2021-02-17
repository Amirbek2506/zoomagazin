using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using ZooMag.Models;
using ZooMag.Models.ViewModels.Categories;
using ZooMag.ViewModels;

namespace ZooMag.Services.Interfaces
{
    public interface ICategoriesService
    {
        Task<Response> Create(InpCategoryModel categoryModel);
        Category FetchById(int id);
        Task<List<Category>> Fetch();
        Task<List<OutCategoryModel>> FetchWithSubcategories();
        Task<Response> Update(UpdCategoryModel categoryModel);
        Task<Response> Delete(int id);
        Task<int> Save();
    }
}