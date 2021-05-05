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
        Task<int> CountProducts(int categoryId, int brandId,int minp,int maxp, bool issale, bool isnew, bool istop, bool isrecommended);
        Task<int> SearchCount(int categoryId,string q);
        Task<Response> DeleteProduct(int id);
        Task<Response> DeleteImage(int id, int productId);
        FirstProductModel FetchProductById(int id);
        Task<List<OutProductModel>> FetchProductByIds(int[] ids);
        Task<Response> SetMainImage(int productid,int imageid);
        Task<List<OutProductModel>> FetchProducts(int rows_limit, int rows_offset, int categoryId, int brandId, int minp, int maxp,bool issale, bool isnew,bool istop,bool isrecommended);
        Task<List<OutProductModel>> FetchSales(int count);
        Task<List<OutProductModel>> FetchTopes(int count);
        Task<List<OutProductModel>> FetchRecommended(int count);
        Task<List<OutProductModel>> FetchNew(int count);
        Task<List<OutProductModel>> Search(int rows_limit, int rows_offset, int categoryId, string q);
        Task CreateProductGaleries(int productId,IFormFile[] images);
        Task<List<ProductImagesModel>> FetchProductGaleriesByProductId(int productId);


        // void Update(int id, string title);
        Task<int> Save();
    }
}