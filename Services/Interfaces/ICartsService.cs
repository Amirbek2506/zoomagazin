using System.Collections.Generic;
using System.Threading.Tasks;
using ZooMag.Models.ViewModels.Carts;
using ZooMag.ViewModels;

namespace ZooMag.Services.Interfaces
{
    public interface ICartsService
    {
        Task<CartModel> Create(InpCartModel model, string userKey);
        Task<List<CartModel>> FetchCartItems(string userKey);
        Task<int> Count(string cartid);
        Task<Response> Delete(int id,string userKey);
        Task<decimal> IncrQty(int id,string userKey);
        Task<decimal> DecrQty(int id,string userKey);
        Task<Response> SetSize(int cartid,int sizeid,string userKey);
    }
}
