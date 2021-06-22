using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ZooMag.Data;
using ZooMag.DTOs.PickupPoint;
using ZooMag.Entities;
using ZooMag.Services.Interfaces;
using ZooMag.ViewModels;

namespace ZooMag.Services
{
    public class PickupPointService : IPickupPointService
    {
        private readonly ApplicationDbContext _context;

        public PickupPointService(ApplicationDbContext context)
        {
            _context = context;
        }
        
        public async Task<Response> CreateAsync(CreatePickupPointRequest request)
        {
            var pickupPoint = new PickupPoint
            {
                Name = request.Name
            };

            await _context.PickupPoints.AddAsync(pickupPoint);
            await _context.SaveChangesAsync();
            return new Response
            {
                Message = "Успешно",
                Status = "success"
            };
        }

        public async Task<List<PickupPointResponse>> GetAllAsync()
        {
            return await _context.PickupPoints.Select(x => new PickupPointResponse { Id = x.Id, Name = x.Name}).ToListAsync();
        }

        public async Task<Response> UpdateAsync(UpdatePickupPointRequest request)
        {
            var pickupPoint = await _context.PickupPoints.FindAsync(request.Id);
            if (pickupPoint == null)
                return new Response
                {
                    Message = "Не найден",
                    Status = "error"
                };
            pickupPoint.Name = request.Name;
            await _context.SaveChangesAsync();
            return new Response
            {
                Message = "Успешно",
                Status = "success"
            };
        }

        public async Task<Response> DeleteAsync(int id)
        {
            var pickupPoint = await _context.PickupPoints.FindAsync(id);
            if (pickupPoint == null)
                return new Response
                {
                    Message = "Не найден",
                    Status = "error"
                };
            _context.PickupPoints.Remove(pickupPoint);
            await _context.SaveChangesAsync();
            return new Response
            {
                Message = "Успешно",
                Status = "success"
            };
        }
    }
}