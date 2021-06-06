using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using ZooMag.Data;
using ZooMag.DTOs.Brand;
using ZooMag.Entities;
using ZooMag.Mapping;
//using ZooMag.Models;
using ZooMag.Models.ViewModels.Categories;
using ZooMag.Services.Interfaces;
using ZooMag.ViewModels;

namespace ZooMag.Services
{
    public class CategoriesService : ICategoriesService
    {
        private readonly ApplicationDbContext _context;

        private readonly IMapper _mapper;

        public CategoriesService(ApplicationDbContext context)
        {
            _context = context;
            _mapper = new MapperConfiguration(x => x.AddProfile<GeneralProfile>()).CreateMapper();
        }

        public async Task<Response> Create(InpCategoryModel categoryModel)
        {
            if (String.IsNullOrEmpty(categoryModel.Title))
                return new Response { Status = "error", Message = "Invalid Category!" };

            var cat = new Category 
            {
                Name = categoryModel.Title,
                ParentCategoryId = categoryModel.ParentId
            };
            _context.Categories.Add(cat);
            await Save();
            if (categoryModel.Image != null)
            {
                string image = await UploadImage(cat.Id, categoryModel.Image);
                cat.ImagePath = "Resources/Images/Categories/" + cat.Id + "/" + image;
                await Save();
            }
            return new Response { Status = "success", Message = "Категория успешно добавлена!" };
        }

        public Category FetchById(int id)
        {
            return _context.Categories.Find(id);
        }

        public async Task<List<Category>> Fetch()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task<Response> Update(UpdCategoryModel categoryModel)
        {
            if (String.IsNullOrEmpty(categoryModel.Title))
            {
                return new Response { Status = "error", Message = "Invalid Category!" };
            }
            var category = await _context.Categories.FindAsync(categoryModel.Id);
            if (category != null)
            {
                category.Name = categoryModel.Title;
                if (categoryModel.Image != null)
                {
                    string image = await UploadImage(categoryModel.Id, categoryModel.Image);
                    category.ImagePath = "Resources/Images/Categories/" + categoryModel.Id + "/" + image;
                }
                await Save();
                return new Response { Status = "success", Message = "Категория успешно изменена!" };
            }
            return new Response { Status = "error", Message = "Категория не существует!" };
        }

        public async Task<Response> Delete(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category != null)
            {
                List<Product> products = await _context.Products.Where(p => p.CategoryId == id).ToListAsync();
                products.ForEach(p => p.CategoryId = (int)category.ParentCategoryId);
                string path = "Resources/Images/Categories/" + category.Id;
                if (Directory.Exists(path))
                    Directory.Delete(path, true);
                _context.Categories.Remove(category);
                var cats = await _context.Categories.Where(x => x.ParentCategoryId == category.Id).ToListAsync();
                cats.ForEach(p => p.ParentCategoryId = category.ParentCategoryId);
                await Save();
                return new Response { Status = "success", Message = "Категория успешно удалена!" };
            }
            else
            {
                return new Response { Status = "error", Message = "Категория не существует!" };
            }
        }


        public async Task<int> Save()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task<List<OutCategoryModel>> FetchWithSubcategories()
        {
            var categories = await _context.Categories.ToListAsync();
            var superCategories = categories.Where(x => x.ParentCategoryId == 0).Select(x => new OutCategoryModel { Id = x.Id, Title = x.Name, Image = x.ImagePath }).ToList();
            foreach (var superCategory in superCategories)
            {
                await GetSubcategories(superCategory, categories);
            }
            return superCategories;
        }

        private async Task GetSubcategories(OutCategoryModel superCategory, IList<Category> categories)
        {
            superCategory.SubCategories = categories.Where(x => x.ParentCategoryId == superCategory.Id).Select(x => new OutCategoryModel { Id = x.Id, Title = x.Name, Image = x.ImagePath }).ToList();
            foreach (var category in superCategory.SubCategories)
            {
                await GetSubcategories(category, categories);
            }
        }

        private async Task<string> UploadImage(int catId, IFormFile file)
        {
            string path = Path.GetFullPath("Resources/Images/Categories/" + catId);
            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
            }
            Directory.CreateDirectory(path);
            path = Path.Combine(path, file.FileName);
            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            return file.FileName;
        }

        public async Task<List<BrandWithoutImageResponse>> GetCategoryBrands(int id)
        {
            return await _context.BrandCategories.Where(x => x.CategoryId == id).Include(x => x.Brand)
                .Select(x => new BrandWithoutImageResponse
                {
                    Id = x.BrandId,
                    Name = x.Brand.Name
                }).ToListAsync();
        }
    }
}