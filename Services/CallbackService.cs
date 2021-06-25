using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZooMag.Data;
using ZooMag.DTOs;
using ZooMag.DTOs.Callback;
using ZooMag.Entities;
using ZooMag.Services.Interfaces;
using ZooMag.ViewModels;

namespace ZooMag.Services
{
    public class CallbackService : ICallbackService
    {
        private readonly ApplicationDbContext _context;

        public CallbackService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Response> CreateAsync(CreateCallbackRequest request)
        {
            var callback = new Callback
            {
                CreateDate = DateTime.Now,
                FromHour = request.FromHour,
                Name = request.Name,
                PhoneNumber = request.PhoneNumber,
                TillHour = request.TillHour
            };

            await _context.Callbacks.AddAsync(callback);

            await _context.SaveChangesAsync();

            return new Response { Status = "success", Message = "Успешно добавлен" };
        }

        public async  Task<List<CallbackResponse>> GetAllAsync(PagedRequest request)
        {
            return await _context.Callbacks.Select(x=> new CallbackResponse 
            { 
                CreateDate = x.CreateDate, 
                FromHour = x.FromHour, 
                Id = x.Id, 
                Name = x.Name, 
                PhoneNumber = x.PhoneNumber, 
                TillHour = x.TillHour
            }).OrderByDescending(x=>x.CreateDate)
                .Skip(request.Offset)
                .Take(request.Limit)
                .ToListAsync();
        }

        public async Task<List<CallbackResponse>> GetAllNewAsync()
        {
            return await _context.Callbacks.Where(x => x.CreateDate.Date == DateTime.Now.Date).Select(x => new CallbackResponse
            {
                CreateDate = x.CreateDate,
                FromHour = x.FromHour,
                Id = x.Id,
                Name = x.Name,
                PhoneNumber = x.PhoneNumber,
                TillHour = x.TillHour
            }).ToListAsync();
        }
    }
}
