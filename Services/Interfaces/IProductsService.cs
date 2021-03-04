using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using ZooMag.Models;
using ZooMag.Models.ViewModels.Products;
using ZooMag.ViewModels;

namespace ZooMag.Services.Interfaces
{
    public interface IProductsService
    {
        Task<int> CreateProduct(InpProductModel product);
        Task<int> UpdateProduct(UpdProductModel product);
        Task<int> CountProducts(int categoryId);
        Task<int> SearchCount(int categoryId,string q);
        Task<Response> DeleteProduct(int id);
        Task<Response> DeleteProductSize(int productId, int sizeId);
        Task<Response> DeleteImage(int id, int productId);
        OutProductModel FetchProductById(int id);
        Task<Response> SetMainImage(int productid,int imageid);
        Task<List<OutProductModel>> FetchProducts(int rows_limit, int rows_offset, int categoryId);
        Task<List<OutProductModel>> FetchSales(int count);
        Task<List<OutProductModel>> FetchNew(int count);
        Task<List<OutProductModel>> Search(int rows_limit, int rows_offset, int categoryId, string q);
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