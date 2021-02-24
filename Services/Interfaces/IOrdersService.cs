using System.Collections.Generic;
using System.Threading.Tasks;
using ZooMag.Models;
using ZooMag.Models.ViewModels.Orders;
using ZooMag.ViewModels;

namespace ZooMag.Services.Interfaces
{
    public interface IOrdersService
    {
        Task<Response> Create(InpOrderModel orderModel, string userKey);
        Task<List<Order>> FetchAll(int offset, int limit);
        Task<List<Order>> FetchMyOrders(string userKey);
        Task<int> Count();
        Task<Response> Delete(int id);
        Task<Response> DeleteItem(int id);
        Task<Response> SetSize(int orderitemid,int sizeid);
        Task<decimal> IncrQty(int itemid);
        Task<decimal> DecrQty(int itemid);
    }
}
