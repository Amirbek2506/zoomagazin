using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using ZooMag.Models;
using ZooMag.ViewModels;

namespace ZooMag.Services.Interfaces
{
    public interface ICategoriesService
    {
        void Create(int parentid, string title);
        int Count();
        Category FetchById(int id);
        Task<List<Category>> Fetch();
        Task<List<CategoryModel>> FetchWithSubcategories();
        Task<Response> Update(int id, string title);
        Task<Response> Delete(int id);
        Task<int> Save();
    }
}