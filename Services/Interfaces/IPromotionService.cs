using System.Collections.Generic;
using System.Threading.Tasks;
using ZooMag.DTOs;
using ZooMag.DTOs.Product;
using ZooMag.DTOs.ProductItem;
using ZooMag.DTOs.Promotion;
using ZooMag.ViewModels;

namespace ZooMag.Services.Interfaces
{
    public interface IPromotionService
    {
        Task<GenericResponse<List<PromotionResponse>>> GetAllAsync(PagedRequest request);
        Task<Response> CreateAsync(CreatePromotionRequest request);
        Task<PromotionResponse> GetByIdAsync(int id);
        Task<GenericResponse<List<ProductResponse>>> GetPromotionProductItemsAsync(GenericPagedRequest<int> request);
        Task DeleteOldPromotionsAsync();
        Task<Response> DeleteAsync(int id);
    }
}