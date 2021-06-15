using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ZooMag.Data;
using ZooMag.DTOs;
using ZooMag.DTOs.Product;
using ZooMag.DTOs.ProductItem;
using ZooMag.DTOs.Promotion;
using ZooMag.Entities;
using ZooMag.Services.Interfaces;
using ZooMag.ViewModels;

namespace ZooMag.Services
{
    public class PromotionService : IPromotionService
    {
        private readonly ApplicationDbContext _context;
        private readonly IFileService _fileService;

        public PromotionService(ApplicationDbContext context, IFileService fileService)
        {
            _context = context;
            _fileService = fileService;
        }
        
        public async Task<GenericResponse<List<PromotionResponse>>> GetAllAsync(PagedRequest request)
        {
            return new ()
            {
                Payload = await _context.Promotions
                    .Skip(request.Offset)
                    .Take(request.Limit)
                    .Select(x => new PromotionResponse
                    {
                        Id = x.Id,
                        Title = x.Title,
                        EndDate = x.EndDate,
                        ImagePath = x.ImagePath,
                        StartDate = x.StartDate
                    }).ToListAsync(),
                Count = await _context.Promotions.CountAsync()
            };
        }

        public async Task<Response> CreateAsync(CreatePromotionRequest request)
        {
            var promotion = new Promotion
            {
                Discount = request.Percent,
                Title = request.Title,
                EndDate = request.EndDate,
                StartDate = request.StartDate,
                ImagePath = await _fileService.AddPromotionFileAsync(request.Image)
            };

            await _context.Promotions.AddAsync(promotion);
            await _context.SaveChangesAsync();
            var productItems =
                await _context.ProductItems.Where(x => request.ProductItems.Contains(x.Id)).ToListAsync();
            for (int i = 0; i < productItems.Count; i++)
            {
                productItems[i].PromotionId = promotion.Id;
                productItems[i].Percent = promotion.Discount;
            }
            _context.ProductItems.UpdateRange(productItems);
            await _context.SaveChangesAsync();
            return new() {Status = "success", Message = "Успешно"};
        }

        public async Task<PromotionResponse> GetByIdAsync(int id)
        {
            return await _context.Promotions.Where(x => x.Id == id).Select(x => new PromotionResponse
            {
                Id = x.Id,
                Title = x.Title,
                EndDate = x.EndDate,
                ImagePath = x.ImagePath,
                StartDate = x.StartDate
            }).FirstOrDefaultAsync();
        }

        public async Task<GenericResponse<List<ProductResponse>>> GetPromotionProductItemsAsync(GenericPagedRequest<int> request)
        {
            return new()
            {
                Payload = await _context.Products.Where(x => !x.Removed).Include(x => x.ProductItems)
                    .ThenInclude(x => x.ProductItemImages)
                    .Where(x => x.ProductItems.Any(pi => !pi.Removed && pi.PromotionId == request.Query))
                    .Skip(request.Offset)
                    .Take(request.Limit)
                    .Select(x => new ProductResponse
                    {
                        Id = x.Id,
                        Title = x.Title,
                        ImagePath = x.ProductItems.First(pi => !pi.Removed).ProductItemImages.First().ImagePath,
                        TitleDescription = x.TitleDescription,
                        ProductItems = x.ProductItems.Where(pi => !pi.Removed).Select(pi => new ProductItemResponse
                        {
                            Id = pi.Id,
                            Discount = pi.Percent,
                            Measure = pi.Measure,
                            Price = pi.Price,
                            SellingPrice = Math.Round(pi.Price - pi.Price * pi.Percent / 100, 2)
                        }).ToList()
                    }).ToListAsync(),
                Count = await _context.Products.Where(x=>!x.Removed).Include(x=>x.ProductItems).Where(x=> x.ProductItems.Any(pi=>!pi.Removed && pi.PromotionId == request.Query)).CountAsync()
            };
        }

        public async Task DeleteOldPromotionsAsync()
        {
            var promotions = await _context.Promotions.Where(x => x.EndDate < DateTime.Now).ToListAsync();
            var promotionIds = promotions.Select(x => x.Id).ToList();
            var promotionProductItems = await _context.ProductItems
                .Where(x => !x.Removed && x.PromotionId.HasValue && promotionIds.Contains(x.PromotionId.Value))
                .ToListAsync();
            for (int i = 0; i < promotionProductItems.Count; i++)
            {
                promotionProductItems[i].Percent = 0;
                promotionProductItems[i].PromotionId = null;
            }
            _context.ProductItems.UpdateRange(promotionProductItems);
            _context.Promotions.RemoveRange(promotions);
            await _context.SaveChangesAsync();
        }
    }
}