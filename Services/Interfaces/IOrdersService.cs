using System.Collections.Generic;
using System.Threading.Tasks;
using ZooMag.DTOs.Order;
using ZooMag.Entities;
using ZooMag.Models;
using ZooMag.Models.ViewModels.Orders;
using ZooMag.ViewModels;

namespace ZooMag.Services.Interfaces
{
    public interface IOrdersService
    {
        Task<Response> CreateAsync(CreateOrderRequest request,int userId);
        // Task<Response> Create(InpOrderModel orderModel, string userKey);
        // Task<List<Order>> FetchAll(int offset, int limit);
        // Task<OutOrderModel> FetchDetail(int orderid);
        // Task<List<Order>> FetchMyOrders(string userKey);
        // Task<int> Count();
        // Task<Response> Delete(int id);
        // Task<Response> ChangeStatus(int id,int statusid);
        // Task<Response> DeleteItem(int id);
        // Task<List<OrderStatus>> FetchStatuses();
        // Task<Response> SetSize(int orderitemid,int sizeid);
        // Task<decimal> IncrQty(int itemid);
        // Task<decimal> DecrQty(int itemid);
    }
}
