using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZooMag.Models;
using ZooMag.Models.ViewModels.Wishlist;
using ZooMag.ViewModels;

namespace ZooMag.Services.Interfaces
{
    public interface IWishlistService
    {
        Task<WishlistModel> Create(Wishlist model);
        Task<List<WishlistModel>> FetchItems(string userKey);
        Task<int> Count(string userKey);
        Task<Response> Delete(int id, string userKey);
    }
}
