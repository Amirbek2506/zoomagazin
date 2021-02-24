using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZooMag.Data;
using ZooMag.Mapping;
using ZooMag.Models;
using ZooMag.Models.ViewModels.Carts;
using ZooMag.Models.ViewModels.Products;
using ZooMag.Services.Interfaces;
using ZooMag.ViewModels;

namespace ZooMag.Services
{
    public class CartsService : ICartsService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;


        public CartsService(ApplicationDbContext context)
        {
            _context = context;
            _mapper = new MapperConfiguration(x => x.AddProfile<GeneralProfile>()).CreateMapper();
        }


        public async Task<CartModel> Create(InpCartModel model, string userKey)
        {
            Product product = await _context.Products.FindAsync(model.ProductId);
            if (product == null)
                return null;
                Cart cart = new Cart
                {
                    UserKey = userKey,
                    ProductId = model.ProductId,
                    Quantity = model.Quantity< 1?1:model.Quantity,
                    SizeId = model.SizeId,
                    Price = model.Quantity <= 1 ? product.SellingPrice : product.SellingPrice * model.Quantity,
                    CreatedAt = DateTime.Now.Date
                };
                _context.Carts.Add(cart);
                await Save();

            var cartmodel = _mapper.Map<Cart, CartModel>(cart);
            cartmodel.Product = _mapper.Map<Product, OutProductModel>(product);
            Size size = await _context.Sizes.FindAsync(model.SizeId);
            if(size!=null)
            {
                cartmodel.Size = size;
            }
            return cartmodel;
        }

        private async Task<int> Save()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task<Response> Delete(int id,string userKey)
        {
            var cart = await _context.Carts.FirstOrDefaultAsync(p=>p.Id == id && p.UserKey == userKey);
            if(cart!=null)
            {
                _context.Carts.Remove(cart);
                await Save();
                return new Response {Status = "success",Message = "Успешно удален!" };
            }
            return new Response { Status = "error", Message = "Не найден!" };
        }

        public async Task<List<CartModel>> FetchCartItems(string userKey)
        {
            var carts = await _context.Carts.Where(p => p.UserKey == userKey).ToListAsync();
            var cartModels = _mapper.Map<List<Cart>, List<CartModel>>(carts);
            foreach(var item in cartModels)
            {
                item.Product = _mapper.Map<Product, OutProductModel>(await _context.Products.FindAsync(item.ProductId));
                item.Size = await _context.Sizes.FindAsync(item.SizeId);
            }
            return cartModels;
        }

        public async Task<int> Count(string cartid)
        {
            return await _context.Carts.CountAsync(p => p.UserKey == cartid);
        }

        public async Task<Response> SetSize(int cartid, int sizeid, string userKey)
        {
            var cart = await _context.Carts.FirstOrDefaultAsync(p => p.Id == cartid && p.UserKey == userKey);
            if (cart != null)
            {
                var prodsize = await _context.ProductSizes
                    .FirstOrDefaultAsync(p=>p.ProductId==cart.ProductId && p.SizeId == sizeid);
                if(prodsize!=null)
                {
                    cart.SizeId = sizeid;
                    await Save();
                    return new Response { Status = "success", Message = "Размер успешно присвоен!" };
                }
                return new Response { Status = "error", Message = "Размер не существует!" };
            }
            return new Response { Status = "error", Message = "Не найден!" };
        }

        public async Task<decimal> IncrQty(int id, string userKey)
        {
            var cart = await _context.Carts.FirstOrDefaultAsync(p=>p.Id == id && p.UserKey == userKey);
            if (cart == null)
            {
                return 0;
            }
            var productPrice = (await _context.Products.FindAsync(cart.ProductId)).SellingPrice;

            cart.Quantity++;
            cart.Price = productPrice * cart.Quantity;
            await Save();
            return cart.Price;
        }

        public async Task<decimal> DecrQty(int id, string userKey)
        {
            var cart = await _context.Carts.FirstOrDefaultAsync(p => p.Id == id && p.UserKey == userKey);
            if (cart == null)
            {
                return 0;
            }
            if(cart.Quantity>1)
            {
                var productPrice = (await _context.Products.FindAsync(cart.ProductId)).SellingPrice;
                cart.Quantity--;
                cart.Price = productPrice * cart.Quantity;
                await Save();
            }
            return cart.Price;
        }
    }
}
