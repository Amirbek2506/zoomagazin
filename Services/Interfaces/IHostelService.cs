using System.Collections.Generic;
using System.Threading.Tasks;
using ZooMag.Models.Entity;
using ZooMag.Models.ViewModels.Hostel;
using ZooMag.ViewModels;

namespace ZooMag.Services.Interfaces
{
    public interface IHostelService
    {
        Task<Response> CreateBox(BoxModel model);
        Task<List<Box>> GetBoxes();
        Task<List<Box>> GetFreeBoxes();
        Task<Response> ChangeStatusBox(int id,string status);
        Task<Response> DeleteBox(int id);
        
        Task<Response> CreateOrder(BoxOrderModel model,int userid);
        Task<List<OutBoxOrderModel>> GetBoxOrders();
        Task<Response> ChangeStatusBoxOrder(int id, string status);
        Task<Response> SetBoxToOrder(int orderid, int boxid);
        Task<Response> DeleteBoxOrder(int id);

        Task<Response> CreateBoxType(BoxTypeModel model);
        Task<List<BoxType>> GetBoxTypes();
        Task<Response> DeleteBoxType(int id);
    }
}