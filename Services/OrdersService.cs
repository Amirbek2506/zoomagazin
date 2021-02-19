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
    public class OrdersService : IOrdersService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;


        public OrdersService(ApplicationDbContext context)
        {
            _context = context;
            _mapper = new MapperConfiguration(x => x.AddProfile<GeneralProfile>()).CreateMapper();
        }

        public async Task<int> Count()
        {
            return await _context.Orders.CountAsync();
        }
       
        public Task<Response> Create(string userKey)
        {
            var order = new Order
            {
                
            };
            throw new NotImplementedException();
        }

        public Task<Response> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Order>> Fetch(string userKey)
        {
            throw new NotImplementedException();
        }

        public Task<List<Order>> FetchAll()
        {
            throw new NotImplementedException();
        }

        public Task<Response> SetSize(int orderitemid, int sizeid, string userKey)
        {
            throw new NotImplementedException();
        }
    }
}
