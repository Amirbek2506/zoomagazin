using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ZooMag.Data;
using ZooMag.DTOs;
using ZooMag.DTOs.Banner;
using ZooMag.Entities;
using ZooMag.Services.Interfaces;
using ZooMag.ViewModels;

namespace ZooMag.Services
{
    public class BannerService : IBannerService
    {
        private readonly ApplicationDbContext _context;
        private readonly IFileService _fileService;

        public BannerService(ApplicationDbContext context, IFileService fileService)
        {
            _context = context;
            _fileService = fileService;
        }
        
        public async Task<Response> CreateAsync(CreateBannerRequest request)
        {
            var banner = new Banner
            {
                PromotionId = request.PromotionId,
                ImagePath = await _fileService.AddBannerFileAsync(request.Image)
            };

            await _context.Banners.AddAsync(banner);
            await _context.SaveChangesAsync();
            return new Response
            {
                Status = "success",
                Message = "Успешно"
            };
        }

        public async Task<Response> UpdateAsync(UpdateBannerRequest request)
        {
            var banner = await _context.Banners.FindAsync(request.Id);
            if (banner == null)
                return new Response
                {
                    Message = "Не найден",
                    Status = "error"
                };
            if (request.Image != null)
            {
                _fileService.Delete(banner.ImagePath);
                banner.ImagePath = await _fileService.AddBannerFileAsync(request.Image);
                await _context.SaveChangesAsync();
            }
            
            return new Response
            {
                Status = "success",
                Message = "Успешно"
            };
        }

        public async Task<Response> DeleteAsync(int id)
        {
            var banner = await _context.Banners.FindAsync(id);
            if (banner == null)
                return new Response
                {
                    Message = "Не найден",
                    Status = "error"
                };
            _fileService.Delete(banner.ImagePath);
            _context.Banners.Remove(banner);
            await _context.SaveChangesAsync();
            return new Response
            {
                Status = "success",
                Message = "Успешно"
            };
        }

        public async Task<List<BannerResponse>> GetAllAsync(PagedRequest request)
        {
            return await _context.Banners.OrderByDescending(x => x.Id).Skip(request.Offset).Take(request.Limit).Select(
                x => new BannerResponse
                {
                    Id = x.Id,
                    ImagePath = x.ImagePath,
                    PromotionId = x.PromotionId
                }).ToListAsync();
        }
    }
}