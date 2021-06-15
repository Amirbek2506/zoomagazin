using System.Collections.Generic;
using System.Threading.Tasks;
using ZooMag.DTOs.Comment;
using ZooMag.ViewModels;

namespace ZooMag.Services.Interfaces
{
    public interface ICommentService
    {
        Task<List<CommentResponse>> GetProductItemReviewsAsync(int productItemId);
        Task<Response> CreateAsync(CreateCommentRequest request);
        Task<List<CommentResponse>> GetAllAsync();
        Task<Response> DeleteAsync(int id);
    }
}