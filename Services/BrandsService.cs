using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ZooMag.Data;
using ZooMag.DTOs;
using ZooMag.DTOs.Brand;
using ZooMag.DTOs.Filter;
using ZooMag.DTOs.FilterCategory;
using ZooMag.DTOs.SpecificFilter;
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

                if (request.BrandCategories != null)
                {
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
                }

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

        public async Task<List<AlphabetCharacterWithBrandsResponse>> GetAllAndOrderingByFirstCharacterAsync()
        {
            var alphabet = new List<string>
            {
                "А", "Б", "В", "Г", "Д", "Е", "Ё", "Ж", "З", "И", "Й", "К", "Л", "М", "Н", "О", "П", "Р", "С", "Т", "У", "Ф", "Х", "Ы", "Ь", "Ъ", "Ш", "Щ", "Э", "Ю", "Я", "Ц", 
                "а", "б", "в", "г", "д", "е", "ё", "ж", "з", "и", "й", "к", "л", "м", "н", "о", "п", "р", "с", "т", "у", "ф", "х", "ы", "ь", "ъ", "ш", "щ", "э", "ю", "я", "ц", 
                "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z",
                "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z"
            };

            var brands = await _context.Brands.ToListAsync();

            return alphabet.Select(x => new AlphabetCharacterWithBrandsResponse
            {
                Character = x,
                Brands = brands.Where(b => b.Name.StartsWith(x)).Select(b => new SimpleBrandResponse
                {
                    Id = b.Id,
                    Name = b.Name
                }).ToList()
            }).Where(x=>x.Brands.Count > 0).OrderBy(x=>x.Character).ToList();
        }

        public async Task<List<FilterCategoryResponse>> GetBrandFiltersAsync(int brandId)
        {
            var brandCategoriesId = await _context.BrandCategories.Where(x => x.BrandId == brandId)
                .Select(x => x.CategoryId).ToListAsync();

            var categories = await _context.Categories.ToListAsync();
            List<int> categoriesId = brandCategoriesId;

            foreach (var i in brandCategoriesId)
            {
                GetParentCategoryCategories(ref categoriesId,i,categories);
            }

            return await _context.FilterCategories.Include(x => x.Filters).ThenInclude(x => x.CategoryFilters)
                .Where(x => x.Filters.Any(f => f.CategoryFilters.Any(cf => categoriesId.Contains(cf.CategoryId))))
                .Select(x => new FilterCategoryResponse
                {
                    Text = x.Text,
                    Filters = x.Filters.Where(f => f.CategoryFilters.Any(cf => categoriesId.Contains(cf.CategoryId)))
                        .Select(f => new FilterResponse
                        {
                            Id = f.Id,
                            Text = f.Text
                        }).ToList()
                }).ToListAsync();
        }

        public async Task<List<SpecificFilterResponse>> GetBrandSpecificFiltersAsync(int brandId)
        {
            var brandCategories = await _context.BrandCategories.Where(x => x.BrandId == brandId)
                .Select(x => x.CategoryId).ToListAsync();
            
            var categories = await _context.Categories.ToListAsync();

            var categoriesId = new List<int>(brandCategories);

            foreach (var i in categoriesId)
            {
                GetParentCategoryCategories(ref categoriesId, i, categories);
            }

            var productIds = await _context.Products.Where(x => categoriesId.Contains(x.CategoryId)).Select(x => x.Id)
                .ToListAsync();

            return await _context.ProductSpecificFilters.Where(x => productIds.Contains(x.ProductId))
                .Include(x => x.SpecificFilter)
                .Select(x => new SpecificFilterResponse
                {
                    Id = x.SpecificFilterId,
                    Text = x.SpecificFilter.Text
                }).ToListAsync();
        }

        private void GetParentCategoryCategories(ref List<int> categoryIds, int parentCategoryId, List<Category> categories)
        {
            var childCategories = categories.Where(x => x.ParentCategoryId == parentCategoryId).Select(x => x.Id)
                .ToList();
            categoryIds.AddRange(childCategories);
            if (childCategories.Count > 0)
            {
                foreach (var categoryId in childCategories)
                {
                    GetParentCategoryCategories(ref categoryIds,categoryId,categories);
                }
            }
        }
    }
}