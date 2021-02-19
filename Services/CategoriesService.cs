using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using ZooMag.Data;
using ZooMag.Mapping;
using ZooMag.Models;
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
            if (String.IsNullOrEmpty(categoryModel.TitleRu) || String.IsNullOrEmpty(categoryModel.TitleEn))
                return new Response { Status = "error", Message = "Invalid Category!" };

            var cat = _mapper.Map<InpCategoryModel, Category>(categoryModel);
            _context.Categories.Add(cat);
            await Save();
            if (categoryModel.Image != null)
            {
                string image = await UploadImage(cat.Id, categoryModel.Image);
                cat.Image = "Resources/Images/Categories/"+ cat.Id + "/" + image;
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
            if (String.IsNullOrEmpty(categoryModel.TitleRu) || String.IsNullOrEmpty(categoryModel.TitleEn))
            {
                return new Response { Status = "error", Message = "Invalid Category!" };
            }
            var category = await _context.Categories.FindAsync(categoryModel.Id);
            if (category != null)
            {
                category.TitleRu = categoryModel.TitleRu;
                category.TitleEn = categoryModel.TitleEn;
                if (categoryModel.Image != null)
                {
                    string image = await UploadImage(categoryModel.Id, categoryModel.Image);
                    category.Image = "Resources/Images/Categories/" + categoryModel.Id + "/" + image;
                }
                await Save();
                return new Response { Status = "success", Message = "Категория успешно изменена!" };
            }
            return new Response { Status = "error", Message = "Категория не существует!" };
        }

        public async Task<Response> Delete(int id)
        {
            var category = FetchById(id);
            if (category != null)
            {
                List<Product> products = await _context.Products.Where(p => p.CategoryId == id).ToListAsync();
                foreach(var product in products)
                {
                    DeleteDirectory(product.Id);
                    await DeleteProductGaleries(id);
                    await DeleteProductSizes(id);
                    product.Image = "Resources/Images/deleted.png";
                    product.IsActive = false;
                }
                string path = "Resources/Images/Categories/" + category.Id;
                if (Directory.Exists(path))
                    Directory.Delete(path, true);
                _context.Categories.Remove(category);
                await Save();
                var cats = await _context.Categories.Where(x => x.ParentId == category.Id).ToListAsync();
                foreach (var cat in cats)
                {
                    await Delete(cat.Id);
                }
                    return new Response { Status = "success", Message = "Категория успешно удалена!" };
            }
            else
            {
                return new Response { Status = "error", Message = "Категория не существует!" };
            }
        }

        private void DeleteDirectory(int productId)
        {
            string path = "Resources/Images/Products/" + productId;
            if (Directory.Exists(path))
                Directory.Delete(path, true);
        }


        public async Task<int> Save()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task<List<OutCategoryModel>> FetchWithSubcategories()
        {
            var categories = await _context.Categories.ToListAsync();
            var superCategories = categories.Where(x => x.ParentId == 0).Select(x=> new OutCategoryModel { Id = x.Id, TitleRu = x.TitleRu,TitleEn = x.TitleEn,Image = x.Image}).ToList();
            foreach(var superCategory in superCategories)
            {
                await GetSubcategories(superCategory, categories);
            }
            return superCategories;
        }

        private async Task GetSubcategories(OutCategoryModel superCategory, IList<Category> categories)
        {
            superCategory.SubCategories = categories.Where(x => x.ParentId == superCategory.Id).Select(x=> new OutCategoryModel { Id = x.Id, TitleRu = x.TitleRu,TitleEn = x.TitleEn,Image = x.Image}).ToList();
            foreach(var category in superCategory.SubCategories)
            {
                await GetSubcategories(category, categories);
            }
        }

        public async Task DeleteProductGaleries(int productId)
        {
            var galeries = await _context.ProductGaleries.Where(p => p.ProductId == productId).ToListAsync();
            _context.ProductGaleries.RemoveRange(galeries);
            await Save();
            return;
        }
        public async Task DeleteProductSizes(int productId)
        {
            var sizes = await _context.ProductSizes.Where(p => p.ProductId == productId).ToListAsync();
            _context.ProductSizes.RemoveRange(sizes);
            await Save();
            return;
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

    }
}