using System.Collections.Generic;
using System.Threading.Tasks;
using ZooMag.Models;
using ZooMag.ViewModels;

namespace ZooMag.Services.Interfaces
{
    public interface IOrdersService
    {
        Task<Response> Create(string userKey);
        Task<List<Order>> FetchAll();
        Task<List<Order>> Fetch(string userKey);
        Task<int> Count();
        Task<Response> Delete(int id);
        Task<Response> SetSize(int orderitemid,int sizeid,string userKey);
    }
}
