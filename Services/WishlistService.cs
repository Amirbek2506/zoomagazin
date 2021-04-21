using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZooMag.Data;
using ZooMag.Mapping;
using ZooMag.Models;
using ZooMag.Models.ViewModels.Products;
using ZooMag.Models.ViewModels.Wishlist;
using ZooMag.Services.Interfaces;
using ZooMag.ViewModels;

namespace ZooMag.Services
{
    public class WishlistService : IWishlistService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public WishlistService(ApplicationDbContext context)
        {
            _context = context;
            _mapper = new MapperConfiguration(x => x.AddProfile<GeneralProfile>()).CreateMapper();
        }

        public async Task<int> Count(string userKey)
        {
            return await _context.Wishlists.CountAsync(p=>p.UserKey == userKey);
        }

        public async Task<WishlistModel> Create(Wishlist model)
        {
            var product = await _context.Products.FindAsync(model.ProductId);
            if(product==null)
            {
                return null;
            }
            if(await _context.Wishlists.Where(p=>p.UserKey==model.UserKey&&p.ProductId==model.ProductId).FirstOrDefaultAsync()!=null)
            {
                return null;
            }
            _context.Wishlists.Add(model);
            await Save();
            return new WishlistModel {Id = model.Id,Product = _mapper.Map<Product, OutProductModel>(product) };
        }

        public async Task<Response> Delete(int id, string userKey)
        {
            var item = await _context.Wishlists.FirstOrDefaultAsync(p => p.Id == id && p.UserKey == userKey);
            if (item != null)
            {
                _context.Wishlists.Remove(item);
                await Save();
                return new Response { Status = "success", Message = "Успешно удален!" };
            }
            return new Response { Status = "error", Message = "Не найден!" };
        }

        public async Task<List<WishlistModel>> FetchItems(string userKey)
        {
            var items = await _context.Wishlists.Where(p => p.UserKey == userKey).ToListAsync();
            List<WishlistModel> wishlist = new List<WishlistModel>();
            foreach (var item in items)
            {
                WishlistModel wishlistmodel =
                    new WishlistModel
                    {
                        Id = item.Id,
                        Product = _mapper.Map<Product, OutProductModel>(await _context.Products.FindAsync(item.ProductId))
                    };
                wishlist.Add(wishlistmodel);
            }
            return wishlist;
        }

        private async Task<int> Save()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
