using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using ZooMag.DTOs;
using ZooMag.DTOs.Product;
using ZooMag.DTOs.ProductItem;
using ZooMag.Models;
using ZooMag.Models.ViewModels.Products;
using ZooMag.ViewModels;

namespace ZooMag.Services.Interfaces
{
    public interface IProductsService
    {
        Task<Response> CreateAsync(CreateProductRequest request);
        Task<Response> UpdateAsync(UpdateProductRequest request);
        Task<Response> DeleteAsync(int id);
        //Task<int> CreateProduct(InpProductModel product);
        //Task<int> UpdateProduct(UpdProductModel product);
        //Task<int> CountProducts(int categoryId, int brandId,int minp,int maxp, bool issale, bool isnew, bool istop, bool isrecommended);
        //Task<int> SearchCount(int categoryId,string q);
        //Task<Response> DeleteProduct(int id);
        //Task<Response> DeleteImage(int id, int productId);
        //FirstProductModel FetchProductById(int id);
        //Task<List<OutProductModel>> FetchProductByIds(int[] ids);
        //Task<Response> SetMainImage(int productid,int imageid);
        //Task<List<OutProductModel>> FetchProducts(FetchProductsRequest request);//int rows_limit, int rows_offset, int categoryId, int brandId, int minp, int maxp,bool issale, bool isnew,bool istop,bool isrecommended);
        //Task<List<OutProductModel>> FetchSales(int count);
        //Task<List<OutProductModel>> FetchTopes(int count);
        //Task<List<OutProductModel>> FetchRecommended(int count);
        //Task<List<OutProductModel>> FetchNew(int count);
        //Task<List<OutProductModel>> Search(int rows_limit, int rows_offset, int categoryId, string q);
        //Task CreateProductGaleries(int productId,IFormFile[] images);
        //Task<List<ProductImagesModel>> FetchProductGaleriesByProductId(int productId);


        //// void Update(int id, string title);
        //Task<int> Save();
        Task<GenericResponse<List<MostPopularProductResponse>>> GetMostPopularAsync(GenericPagedRequest<int> request);
        Task<SearchResponse> SearchAsync(GenericPagedRequest<string> request);
        Task<GenericResponse< List<ProductResponse>>> GetAllAsync(PagedRequest request);
        Task<List<WishListProductItemResponse>> GetWishListAsync(string key);
        Task<int> GetWishlistCountAsync(string key);
        Task<Response> AddToWishlistAsync(string key, int productItemId);
        Task<Response> DeleteFromWishlistAsync(string key, int productItemId);
        Task<Response> AddToBasketAsync(string key,AddToBusketRequest request);
        Task<List<BasketProductResponse>> GetBasketProductsAsync(string key);
        Task ChangeWishlistProductsUserIdAsync(string key,string newKey);
        Task ChangeBasketProductsUserIdAsync(string key,string newKey);
        Task<Response> DeleteBasketProductAsync(int productItemId,string key);
        Task<Response> DecreaseBasketProductAsync(int productItemId, string key);
        Task<GenericResponse< List<ProductResponse>>> GetFilteredProductsAsync(GenericPagedRequest<ProductFiltersRequest> request);
        Task<ProductItemDetailsResponse> GetProductItemDetailsAsync(int productItemId);
        Task<ProductDetailsResponse> GetProductDetailsAsync(int id);
        Task<GenericResponse< List<ProductResponse>>> GetProductsByBrandIdAsync(GenericPagedRequest<int> request);
    }
}