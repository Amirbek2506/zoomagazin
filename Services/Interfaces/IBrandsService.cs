using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using ZooMag.Models;
using ZooMag.Models.Entity;
using ZooMag.Models.ViewModels.Brands;
using ZooMag.Models.ViewModels.Categories;
using ZooMag.ViewModels;

namespace ZooMag.Services.Interfaces
{
    public interface IBrandsService
    {
        Task<Response> Create(InpBrandModel model);
        Task<List<Brand>> Fetch();
        Brand FetchById(int Id);
        Task<Response> Update(UpdBrandModel model);
        Task<Response> Delete(int id);
    }
}