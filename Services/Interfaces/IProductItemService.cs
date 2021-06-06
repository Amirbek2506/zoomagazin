using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZooMag.DTOs.ProductItem;
using ZooMag.ViewModels;

namespace ZooMag.Services.Interfaces
{
    public interface IProductItemService
    {
        Task<Response> UpdateAsync(UpdateProductItemRequest request);
        Task<Response> DeleteAsync(int id);
        Task<Response> CreateAsync(CreateProductItemRequest request);
    }
}
