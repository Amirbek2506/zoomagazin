using System.Collections.Generic;
using System.Threading.Tasks;
using ZooMag.DTOs;
using ZooMag.DTOs.Callback;
using ZooMag.ViewModels;

namespace ZooMag.Services.Interfaces
{
    public interface ICallbackService
    {
        Task<Response> CreateAsync(CreateCallbackRequest request);
        Task<List<CallbackResponse>> GetAllAsync(PagedRequest request);
        Task<List<CallbackResponse>> GetAllNewAsync();
    }
}
