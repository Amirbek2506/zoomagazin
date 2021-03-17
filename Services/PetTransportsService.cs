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
using ZooMag.Models.ViewModels.PetTransports;
using ZooMag.Services.Interfaces;
using ZooMag.ViewModels;

namespace ZooMag.Services
{
    public class PetTransportsService : IPetTransportsService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;


        public PetTransportsService(ApplicationDbContext context)
        {
            _context = context;
            _mapper = new MapperConfiguration(x => x.AddProfile<GeneralProfile>()).CreateMapper();
        }

        public async Task<Response> Create(InpPetTransportModel model, int userid)
        {
            try
            {
                PetTransport petTransport = _mapper.Map<InpPetTransportModel, PetTransport>(model);
                petTransport.CreatedAt = DateTime.Now;
                petTransport.UserId = userid;
                petTransport.OrderStatusId = 1;
                _context.PetTransports.Add(petTransport);
                await Save();
                return new Response { Status = "success", Message = "Успешно добавлен!" };
            }
            catch (Exception ex)
            {
                return new Response { Status = "error", Message = ex.Message };
            }
        }

        public async Task<List<OutPetTransport>> Get()
        {
            var petTransports = await _context.PetTransports.ToListAsync();
            var orders = new List<OutPetTransport>();
            foreach (var item in petTransports)
            {
                item.AnimalType = await _context.AnimalTypes.FindAsync(item.AnimalTypeId);
                item.OrderStatus = await _context.OrderStatuses.FindAsync(item.OrderStatusId);

                var order = _mapper.Map<PetTransport, OutPetTransport>(item);
                order.User = _mapper.Map<User, UserModel>(await _context.Users.FindAsync(item.UserId));
                order.AnimalType.PetTransports = null;
                order.OrderStatus.PetTransports = null;
                orders.Add(order);
            }
            return orders;
        }

        public async Task<OutPetTransport> GetById(int id)
        {
            var petTransport = await _context.PetTransports.FindAsync(id);
            if (petTransport == null)
            {
                return null;
            }
            petTransport.AnimalType = await _context.AnimalTypes.FindAsync(petTransport.AnimalTypeId);
            petTransport.OrderStatus = await _context.OrderStatuses.FindAsync(petTransport.OrderStatusId);
            var order = _mapper.Map<PetTransport, OutPetTransport>(petTransport);
            order.User = _mapper.Map<User, UserModel>(await _context.Users.FindAsync(petTransport.UserId));
            order.AnimalType.PetTransports = null;
            order.OrderStatus.PetTransports = null;
            return order;
        }

        public async Task<Response> ChangeStatus(int id, int statusid)
        {
            var petTransport = await _context.PetTransports.FindAsync(id);
            if (petTransport == null)
            {
                return new Response { Status = "error", Message = "Заявка не найдено!" };
            }
            var status = await _context.OrderStatuses.FindAsync(statusid);
            if (status == null)
            {
                return new Response { Status = "error", Message = "Статус не найден!" };
            }
            petTransport.OrderStatusId = statusid;
            await Save();
            return new Response { Status = "success", Message = "Статус успешно присвоен!" };
        }


        public async Task<Response> Delete(int id)
        {
            PetTransport petTransport = await _context.PetTransports.FindAsync(id);
            if (petTransport == null)
            {
                return new Response { Status = "error", Message = "Не найден!" };
            }
            _context.PetTransports.Remove(petTransport);
            await Save();
            return new Response { Status = "success", Message = "Успешно удален!" };
        }


        private async Task<int> Save()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
