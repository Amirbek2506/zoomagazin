using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ZooMag.Data;
using ZooMag.DTOs.Shop;
using ZooMag.Entities;
using ZooMag.Services.Interfaces;
using ZooMag.ViewModels;

namespace ZooMag.Services
{
    public class ShopService : IShopService
    {
        private readonly ApplicationDbContext _context;

        public ShopService(ApplicationDbContext context)
        {
            _context = context;
        }
        
        public async Task<Response> CreateAsync(CreateShopRequest request)
        {
            var pickupPoint = new Shop
            {
                Name = request.Name,
                Address = request.Address,
                Graphic = request.Graphic,
                PhoneNumber = request.PhoneNumber,
                CityId = request.CityId,
                Latitude = request.Latitude,
                Longitude = request.Longitude
            };

            await _context.Shops.AddAsync(pickupPoint);
            await _context.SaveChangesAsync();
            return new Response
            {
                Message = "Успешно",
                Status = "success"
            };
        }

        public async Task<List<PickupPointResponse>> GetAllPickupPointsAsync()
        {
            return await _context.Shops.Select(x => new PickupPointResponse
            {
                Id = x.Id,
                Name = x.Name,
                Address = x.Address,
                Latitude = x.Latitude,
                Longitude = x.Longitude
            }).ToListAsync();
        }

        public async Task<List<ShopResponse>> GetAllAsync()
        {
            return await _context.Shops.Select(x => new ShopResponse
            {
                Id = x.Id, 
                Name = x.Name,
                Address = x.Address,
                Graphic = x.Graphic,
                PhoneNumber = x.PhoneNumber
            }).ToListAsync();
        }

        public async Task<Response> UpdateAsync(UpdateShopRequest request)
        {
            var shop = await _context.Shops.FindAsync(request.Id);
            if (shop == null)
                return new Response
                {
                    Message = "Не найден",
                    Status = "error"
                };
            shop.Name = request.Name;
            shop.Address = request.Address;
            shop.Graphic = request.Graphic;
            shop.PhoneNumber = request.PhoneNumber;
            shop.Latitude = request.Latitude;
            shop.Longitude = request.Longitude;
            shop.CityId = request.CityId;
            await _context.SaveChangesAsync();
            return new Response
            {
                Message = "Успешно",
                Status = "success"
            };
        }

        public async Task<Response> DeleteAsync(int id)
        {
            var shop = await _context.Shops.FindAsync(id);
            if (shop == null)
                return new Response
                {
                    Message = "Не найден",
                    Status = "error"
                };
            _context.Shops.Remove(shop);
            await _context.SaveChangesAsync();
            return new Response
            {
                Message = "Успешно",
                Status = "success"
            };
        }

        public async Task<List<PickupPointResponse>> GetAllPickupPointsByCityIdAsync(int cityId)
        {
            return await _context.Shops
                .Where(x=> x.CityId == cityId)
                .Select(x => new PickupPointResponse
                {
                    Id = x.Id,
                    Name = x.Name,
                    Address = x.Address,
                    Latitude = x.Latitude,
                    Longitude = x.Longitude
                }).ToListAsync();
        }

        public async Task<ShopResponse> GetByIdAsync(int shopId)
        {
            return await _context.Shops.Where(x=>x.Id == shopId).Select(x=> new ShopResponse
            {
                Address = x.Address,
                Graphic = x.Graphic,
                Id = x.Id,
                Latitude = x.Latitude,
                Longitude = x.Longitude,
                Name = x.Name,
                PhoneNumber = x.PhoneNumber
            }).FirstOrDefaultAsync();
            
        }
    }
}