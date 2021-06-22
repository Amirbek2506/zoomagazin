using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ZooMag.Data;
using ZooMag.DTOs.Brand;
using ZooMag.Entities;
using ZooMag.Services.Interfaces;
using ZooMag.ViewModels;

namespace ZooMag.Services
{
    public class BrandsService : IBrandsService
    {
        private readonly ApplicationDbContext _context;
        private readonly IFileService _fileService;

        public BrandsService(ApplicationDbContext context, IFileService fileService)
        {
            _context = context;
            _fileService = fileService;
        }

        //Создание бренда с его категориями и добавление файла если он был принят
        public async Task<Response> CreateAsync(CreateBrandRequest request)
        {
            var brand = new Brand
            {
                Name = request.Name,
                ImagePath = request.Image != null ? await _fileService.AddBrandFileAsync(request.Image) : default,
                BrandCategories = request.BrandCategories?.Select(x=> new BrandCategory{ CategoryId = x}).ToList() ?? new List<BrandCategory>()
            };

            await _context.Brands.AddAsync(brand);

            await _context.SaveChangesAsync();

            return new Response { Status = "success", Message = "Бренд успешно добавлена!" };
        }

        //Взятие всех брендов
        public async Task<List<BrandResponse>> GetAllAsync()
        {
            return await _context.Brands.Select(x => new BrandResponse
            {
                Id = x.Id,
                ImagePath = x.ImagePath,
                Name = x.Name
            }).OrderByDescending(x=>x.Id).ToListAsync();
        }

        //Взятие всех брендов с их категориями
        public async Task<List<BrandWithCategoryResponse>> GetAllWithCategoriesAsync()
        {
            return await _context.Brands
                .Include(x => x.BrandCategories)
                .ThenInclude(x => x.Category)
                .Select(x => new BrandWithCategoryResponse
                {
                    Id = x.Id,
                    ImagePath = x.ImagePath,
                    Name = x.Name,
                    BrandCategories = x.BrandCategories.Select(c=> new BrandCategoryResponse 
                    {
                        CategoryId = c.CategoryId,
                        Name = c.Category.Name,
                        ImagePath = c.Category.ImagePath
                    }).ToList()
                })
                .OrderByDescending(x=>x.Id)
                .ToListAsync();
        }

        //Взятие бренда по идентификатору
        public async Task<BrandResponse> GetByIdAsync(int id)
        {
            return await _context.Brands.Select(x => new BrandResponse
            {
                Id = x.Id,
                ImagePath = x.ImagePath,
                Name = x.Name
            }).FirstOrDefaultAsync(x => x.Id == id);
        }

        //Взятие категориев бренда по идентификатору бренда
        public async Task<List<int>> GetBrandCategoriesAsync(int id)
        {
            return await _context.BrandCategories.Select(x => x.CategoryId).ToListAsync();
        }

        //Обновление бренда, списка его категориев и фото в случае принятия файла
        public async Task<Response> UpdateAsync(UpdateBrandRequest request)
        {
            var brand = await _context.Brands.FindAsync(request.Id);

            if(brand != null)
            {
                brand.Name = request.Name;

                var brandCategories = await _context.BrandCategories.Where(x => x.BrandId == request.Id).ToListAsync();

                var brandCategoriesId = brandCategories.Select(x => x.CategoryId).ToList();

                var oldBrandCategoriesId = brandCategoriesId.Except(request.BrandCategories);

                var newBrandCategoriesId = request.BrandCategories.Except(brandCategoriesId);

                var newBrandCategories = newBrandCategoriesId.Select(x => new BrandCategory
                {
                    BrandId = request.Id,
                    CategoryId = x
                });

                var oldBrandCategories = brandCategories.Where(x => oldBrandCategoriesId.Contains(x.CategoryId));

                _context.BrandCategories.RemoveRange(oldBrandCategories);
                await _context.BrandCategories.AddRangeAsync(newBrandCategories);

                if(request.Image != null)
                {
                    _fileService.Delete(brand.ImagePath);
                    brand.ImagePath = await _fileService.AddBrandFileAsync(request.Image);
                }

                await _context.SaveChangesAsync();
                return new Response { Status = "success", Message = "Бренд успешно изменен!" };
            }

            return new Response { Status = "error", Message = "Бренд не существует!" };
        }

        //Удаление всех категориев, продуктов и самого бренда по идентификатору бренда
        public async Task<Response> DeleteAsync(int id)
        {
            var brand = await _context.Brands.FindAsync(id);
            if (brand != null)
            {
                _fileService.Delete(brand.ImagePath);
                var brandProducts = await _context.Products.Where(x => x.BrandId == id).ToListAsync();
                var brandCategories = await _context.BrandCategories.Where(x => x.BrandId == id).ToListAsync();
                _context.BrandCategories.RemoveRange(brandCategories);
                _context.Products.RemoveRange(brandProducts);
                _context.Brands.Remove(brand);
                await _context.SaveChangesAsync();
                return new Response { Status = "success", Message = "Бренд успешно удален!" };
            }
            return new Response { Status = "error", Message = "Бренд не существует!" };
        }
    }
}