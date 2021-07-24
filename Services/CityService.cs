using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ZooMag.Data;
using ZooMag.DTOs;
using ZooMag.DTOs.City;
using ZooMag.Entities;
using ZooMag.Services.Interfaces;
using ZooMag.ViewModels;

namespace ZooMag.Services
{
    public class CityService : ICityService
    {
        private readonly ApplicationDbContext _context;

        public CityService(ApplicationDbContext context)
        {
            _context = context;
        }
        
        public async Task<Response> CreateAsync(CreateCityRequest request)
        {
            var city = new City
            {
                Name = request.Name
            };

            await _context.Cities.AddAsync(city);
            await _context.SaveChangesAsync();
            return new Response
            {
                Status = "success",
                Message = "Успешно"
            };
        }

        public async Task<GenericResponse<List<CityResponse>>> GetAllAsync(PagedRequest request)
        {
            return new()
            {
                Payload = await _context.Cities.OrderBy(x => x.Name)
                    .Skip(request.Offset)
                    .Take(request.Limit)
                    .Select(x => new CityResponse
                    {
                        Id = x.Id,
                        Name = x.Name
                    }).ToListAsync(),
                Count = await _context.Cities.CountAsync()
            };
        }

        public async Task<Response> UpdateAsync(UpdateCityRequest request)
        {
            var city = await _context.Cities.FindAsync(request.Id);
            if (city == null)
                return new Response
                {
                    Status = "error",
                    Message = "Не найден"
                };
            city.Name = request.Name;
            await _context.SaveChangesAsync();
            return new Response
            {
                Status = "success",
                Message = "Успешно"
            };
        }

        public async Task<Response> DeleteAsync(int id)
        {
            var city = await _context.Cities.FindAsync(id);
            if (city == null)
                return new Response
                {
                    Status = "error",
                    Message = "Не найден"
                };
            _context.Cities.Remove(city);
            await _context.SaveChangesAsync();
            return new Response
            {
                Status = "success",
                Message = "Успешно"
            };
        }
    }
}