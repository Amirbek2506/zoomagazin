using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZooMag.Data;
using ZooMag.Mapping;
using ZooMag.Models;
using ZooMag.Services.Interfaces;
using ZooMag.ViewModels;

namespace ZooMag.Services
{
    public class CartService : ICartService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;


        public CartService(ApplicationDbContext context)
        {
            _context = context;
            _mapper = new MapperConfiguration(x => x.AddProfile<GeneralProfile>()).CreateMapper();
        }


        public async Task<Cart> Add(int productId,string cartid, int quantity = 1, int sizeId = 0)
        {

            Product product = await _context.Products.FindAsync(productId);
                Cart cart = new Cart
                {
                    UserKey = cartid,
                    ProductId = productId,
                    Quantity = quantity,
                    SizeId = sizeId,
                    Price = product.SellingPrice * quantity,
                    CreatedAt = DateTime.Now.Date
                };

                _context.Carts.Add(cart);
                await Save();
                cart.Product = product;
            return cart;
        }

        private async Task<int> Save()
        {
            return await _context.SaveChangesAsync();
        }

        public Task<Response> Delete()
        {
            throw new NotImplementedException();
        }

        public Task<Response> Add()
        {
            throw new NotImplementedException();
        }
    }
}
