using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using ZooMag.Data;
using ZooMag.Mapping;
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

        public async Task<Response> Create(InpPetTransportModel model,int userid)
        {
            try
            {
                PetTransport petTransport = _mapper.Map<InpPetTransportModel, PetTransport>(model);
                petTransport.CreatedAt = DateTime.Now;
                petTransport.UserId = userid;
                _context.PetTransports.Add(petTransport);
                await Save();
                return new Response { Status = "success", Message = "Успешно добавлен!" };
            }
            catch(Exception ex)
            {
                return new Response { Status = "error", Message = ex.Message };
            }
        }

        public async Task<List<PetTransport>> Get()
        {
            var PetTransports = await _context.PetTransports.ToListAsync();
            foreach(var item in PetTransports)
            {
                item.AnimalType = await _context.AnimalTypes.FindAsync(item.AnimalTypeId);
                item.OrderStatus = await _context.OrderStatuses.FindAsync(item.OrderStatusId);
                item.User = null;
            }
            return PetTransports;
        }

        public async Task<PetTransport> GetById(int id)
        {
            var PetTransport = await _context.PetTransports.FindAsync(id);
            PetTransport.AnimalType = await _context.AnimalTypes.FindAsync(PetTransport.AnimalTypeId);
            PetTransport.OrderStatus = await _context.OrderStatuses.FindAsync(PetTransport.OrderStatusId);
            PetTransport.User = null;
            return PetTransport;
        }

        public async Task<Response> ChangeStatus(int id, int statusid)
        {
            try
            {
                var petTransport = await _context.PetTransports.FindAsync(id);
                if (petTransport == null)
                {
                    return new Response { Status = "error", Message = "Заявка не найдено!" };
                }
                petTransport.OrderStatusId = statusid;
                await Save();
                return new Response { Status = "success", Message = "Статус успешно присвоен!" };
            }
            catch(Exception ex)
            {
                return new Response { Status = "error", Message = ex.Message };
            }
        }


        public async Task<Response> Delete(int id)
        {
            PetTransport petTransport = await _context.PetTransports.FindAsync(id);
            if(petTransport == null)
            {
                return new Response { Status = "error", Message = "Не найден!" };
            }
            _context.PetTransports.Remove(petTransport);
            await Save();
            return new Response {Status = "success",Message = "Успешно удален!" };
        }


        private async Task<int> Save()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
