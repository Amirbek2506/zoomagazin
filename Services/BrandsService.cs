/*using System;
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
using ZooMag.Models.Entity;
using ZooMag.Models.ViewModels.Brands;
using ZooMag.Models.ViewModels.Categories;
using ZooMag.Services.Interfaces;
using ZooMag.ViewModels;

namespace ZooMag.Services
{
    public class BrandsService : IBrandsService
    {
        private readonly ApplicationDbContext _context;

        private readonly IMapper _mapper;

        public BrandsService(ApplicationDbContext context)
        {
            _context = context;
            _mapper = new MapperConfiguration(x => x.AddProfile<GeneralProfile>()).CreateMapper();
        }

        public async Task<Response> Create(InpBrandModel model)
        {
            if (String.IsNullOrEmpty(model.Title))
                return new Response { Status = "error", Message = "Invalid model!" };

            var brand = _mapper.Map<InpBrandModel, Brand>(model);
            _context.Brands.Add(brand);
            await Save();
            if (model.Image != null)
            {
                string image = await UploadImage(brand.Id, model.Image);
                brand.Image = "Resources/Images/Brands/"+ brand.Id + "/" + image;
                await Save();
            }
            return new Response { Status = "success", Message = "Бренд успешно добавлена!" };
        }

        public Brand FetchById(int id)
        {
            return _context.Brands.Find(id);
        }

        public async Task<List<Brand>> Fetch()
        {
            return await _context.Brands.ToListAsync();
        }

        public async Task<Response> Update(UpdBrandModel model)
        {
            if (String.IsNullOrEmpty(model.Title))
            {
                return new Response { Status = "error", Message = "Invalid model!" };
            }
            var brand = await _context.Brands.FindAsync(model.Id);
            if (brand != null)
            {
                brand.Title = model.Title;
                if (model.Image != null)
                {
                    string image = await UploadImage(model.Id, model.Image);
                    brand.Image = "Resources/Images/Brands/" + model.Id + "/" + image;
                }
                await Save();
                return new Response { Status = "success", Message = "Бренд успешно изменена!" };
            }
            return new Response { Status = "error", Message = "Бренд не существует!" };
        }

        public async Task<Response> Delete(int id)
        {
            var brand = await _context.Brands.FindAsync(id);
            if (brand != null)
            {
                List<Product> products = await _context.Products.Where(p => p.BrandId == id).ToListAsync();
                products.ForEach(p=>p.BrandId = 0);
                string path = "Resources/Images/Brands/" + brand.Id;
                if (Directory.Exists(path))
                    Directory.Delete(path, true);
                _context.Brands.Remove(brand);
                await Save();
                    return new Response { Status = "success", Message = "Бренд успешно удалена!" };
            }
            else
            {
                return new Response { Status = "error", Message = "Бренд не существует!" };
            }
        }

        public async Task<int> Save()
        {
            return await _context.SaveChangesAsync();
        }

        private async Task<string> UploadImage(int Id, IFormFile file)
        {
            string path = Path.GetFullPath("Resources/Images/Brands/" + Id);
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
}*/