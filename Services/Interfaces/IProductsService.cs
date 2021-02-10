using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using ZooMag.Models;
using ZooMag.ViewModels;

namespace ZooMag.Services.Interfaces
{
    public interface IProductsService
    {
        int CreateProduct(ProductModel product);
        Task<int> UpdateProduct(ProductModel product);
        int CountProducts();
        Task<Response> DeleteProduct(int id);
        ProductModel FetchProductById(int id);
        Task<List<ProductModel>> FetchProducts();
        Task<List<int>> CreateSizes(List<string> sizes);
        Task CreateProductSizes(int productId,List<int> sizeIds);
        Task CreateProductGaleries(int productId,IFormFile[] images);
        Task<List<ProductImagesModel>> FetchProductGaleriesByProductId(int productId);
        Task<List<ProductSize>> FetchProductSizesByProductId(int productId);
        Task<List<SizeModel>> FetchSizesByProductId(int productId);


        // void Update(int id, string title);
        Task<int> Save();
    }
}