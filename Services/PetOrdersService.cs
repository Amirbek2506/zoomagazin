using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using ZooMag.Data;
using ZooMag.Mapping;
using ZooMag.Models;
using ZooMag.Models.Entity;
using ZooMag.Models.ViewModels.Articles;
using ZooMag.Models.ViewModels.PetOrders;
using ZooMag.Models.ViewModels.PetTransports;
using ZooMag.Services.Interfaces;
using ZooMag.ViewModels;

namespace ZooMag.Services
{
    public class PetOrdersService : IPetOrdersService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;


        public PetOrdersService(ApplicationDbContext context)
        {
            _context = context;
            _mapper = new MapperConfiguration(x => x.AddProfile<GeneralProfile>()).CreateMapper();
        }

        public async Task<Response> Create(PetOrderModel model)
        {
            if(String.IsNullOrEmpty(model.Details))
            {
                return new Response {Status = "error",Message = "Заказ не коректно!"};
            }
            var petorder = new PetOrder
            {
                Details = model.Details,
                PhoneNumber = model.PhoneNumber,
                CreatedAt = DateTime.Now
            };
            _context.PetOrders.Add(petorder);
            await Save();
            return new Response {Status = "success",Message = "Успешно добавлен!"};
        }

        public async Task<Response> Delete(int id)
        {
            var petorder = await _context.PetOrders.FindAsync(id);
            if(petorder==null)
            {
                return new Response { Status = "error", Message = "Не найден!" };
            }
            _context.PetOrders.Remove(petorder);
            await Save();
            return new Response {Status = "success",Message = "Успешно удален!"};
        }

        public async Task<List<PetOrder>> Get()
        {
            return await _context.PetOrders.ToListAsync();
        }

        private async Task<int> Save()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
