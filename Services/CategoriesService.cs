using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using ZooMag.Data;
using ZooMag.Models;
using ZooMag.Services.Interfaces;
using ZooMag.ViewModels;

namespace ZooMag.Services
{
    public class CategoriesService : ICategoriesService
    {
        private readonly ApplicationDbContext _context;

        public CategoriesService(ApplicationDbContext context)
        {
            _context = context;
        }

        public int Count()
        {
            return _context.Categories.Count();
        }

        public Category FetchById(int id)
        {
            return _context.Categories.Find(id);
        }

        public async Task<List<Category>> Fetch()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task<Response> Update(int id, string title)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(p=>p.Id==id);
            if (category != null)
            {
                category.Title = title;
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

        public void Create(int parentid, string title)
        {
            _context.Categories.Add(new Category { ParentId = parentid,Title = title});
        }

        public async Task<int> Save()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task<List<CategoryModel>> FetchWithSubcategories()
        {
            var categories = await _context.Categories.ToListAsync();
            var superCategories = categories.Where(x => x.ParentId == 0).Select(x=> new CategoryModel { Id = x.Id, Title = x.Title}).ToList();
            foreach(var superCategory in superCategories)
            {
                await GetSubcategories(superCategory, categories);
            }
            return superCategories;
        }

        private async Task GetSubcategories(CategoryModel superCategory, IList<Category> categories)
        {
            superCategory.SubCategories = categories.Where(x => x.ParentId == superCategory.Id).Select(x=> new CategoryModel { Id = x.Id, Title = x.Title}).ToList();
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

    }
}