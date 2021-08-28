using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using ZooMag.Data;
using ZooMag.DTOs.PetCategory;
using ZooMag.Entities;
using ZooMag.Mapping;
using ZooMag.Models;
using ZooMag.Models.ViewModels.Categories;
using ZooMag.Models.ViewModels.PetCategories;
using ZooMag.Services.Interfaces;
using ZooMag.ViewModels;

namespace ZooMag.Services
{
    public class PetCategoriesService : IPetCategoriesService
    {
        private readonly ApplicationDbContext _context;

        private readonly IMapper _mapper;

        public PetCategoriesService(ApplicationDbContext context)
        {
            _context = context;
            _mapper = new MapperConfiguration(x => x.AddProfile<GeneralProfile>()).CreateMapper();
        }

        public async Task<Response> Create(InpPetCategoryModel model)
        {
            if (String.IsNullOrEmpty(model.Title))
                return new Response { Status = "error", Message = "Invalid model!" };

            var cat = _mapper.Map<InpPetCategoryModel, PetCategory>(model);
            _context.PetCategories.Add(cat);
            await Save();
            if (model.Image != null)
            {
                string image = await UploadImage(cat.Id, model.Image);
                cat.CategoryImage = "Resources/Images/PetCategories/"+ cat.Id + "/" + image;
                await Save();
            }
            return new Response { Status = "success", Message = "Категория успешно добавлена!" };
        }

        public PetCategory FetchById(int id)
        {
            return _context.PetCategories.Find(id);
        }
        public async Task<Response> Update(UpdPetCategoryModel model)
        {
            if (String.IsNullOrEmpty(model.Title))
            {
                return new Response { Status = "error", Message = "Invalid Category!" };
            }
            var category = await _context.PetCategories.FindAsync(model.Id);
            if (category != null)
            {
                category.Title = model.Title;
                if (model.Image != null)
                {
                    string image = await UploadImage(model.Id, model.Image);
                    category.CategoryImage = "Resources/Images/PetCategories/" + model.Id + "/" + image;
                }
                await Save();
                return new Response { Status = "success", Message = "Категория успешно изменена!" };
            }
            return new Response { Status = "error", Message = "Категория не существует!" };
        }

        public async Task<Response> Delete(int id)
        {
            var category = await _context.PetCategories.FindAsync(id);
            if (category != null)
            {
                List<Pet> pets = await _context.Pets.Where(p => p.PetCategoryId == id).ToListAsync();
                pets.ForEach(p=>p.PetCategoryId = category.ParentId??_context.PetCategories.First().Id);
                string path = "Resources/Images/PetCategories/" + category.Id;
                if (Directory.Exists(path))
                    Directory.Delete(path, true);
                _context.PetCategories.Remove(category);
                var cats = await _context.PetCategories.Where(x => x.ParentId == category.Id).ToListAsync();
                cats.ForEach(p=>p.ParentId = category.ParentId);
                await Save();
                    return new Response { Status = "success", Message = "Категория успешно удалена!" };
            }
            else
            {
                return new Response { Status = "error", Message = "Категория не существует!" };
            }
        }


        private async Task<int> Save()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task<List<OutPetCategoryModel>> FetchWithSubcategories()
        {
            var categories = await _context.PetCategories.ToListAsync();
            var superCategories = categories.Where(x => x.ParentId == null).Select(x=> new OutPetCategoryModel { Id = x.Id, Title = x.Title,Image = x.CategoryImage}).ToList();
            foreach(var superCategory in superCategories)
            {
                await GetSubcategories(superCategory, categories);
            }
            return superCategories;
        }

        private async Task GetSubcategories(OutPetCategoryModel superCategory, IList<PetCategory> categories)
        {
            superCategory.SubCategories = categories.Where(x => x.ParentId == superCategory.Id).Select(x=> new OutPetCategoryModel { Id = x.Id, Title = x.Title,Image = x.CategoryImage}).ToList();
            foreach(var category in superCategory.SubCategories)
            {
                await GetSubcategories(category, categories);
            }
        }

        private async Task<string> UploadImage(int catId, IFormFile file)
        {
            string path = Path.GetFullPath("Resources/Images/PetCategories/" + catId);
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
        public async Task<List<GetPetCategoryItemRequest>> Fetch()
        {
            var entities = await _context.PetCategories.ToListAsync();
            var result = _mapper.Map<List<PetCategory>, List<GetPetCategoryItemRequest>>(entities);
            return result;
        }
    }
}