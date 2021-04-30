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
using ZooMag.Models.ViewModels.SlideShows;
using ZooMag.Services.Interfaces;
using ZooMag.ViewModels;

namespace ZooMag.Services
{
    public class SlideShowsService : ISlideShowsService
    {
        private readonly ApplicationDbContext _context;

        public SlideShowsService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Response> Create(InpSlideShowModel model)
        {
            if (String.IsNullOrEmpty(model.Category) || model.Image == null || model.ImageMobile == null)
                return new Response { Status = "error", Message = "Invalid model!" };
            var slide = new SlideShow
            {
                Category = model.Category
            };

            _context.SlideShows.Add(slide);
            await Save();

            string image = await UploadImage(slide.Id, model.Image,"");
            slide.Image = "Resources/Images/SlideShows/"+ slide.Id + "/" + image;

            string imageMobile = await UploadImage(slide.Id, model.ImageMobile,"");
            slide.ImageMobile = "Resources/Images/SlideShows/" + slide.Id + "/" + imageMobile;

            await Save();

            return new Response { Status = "success", Message = "Слайд успешно добавлена!" };
        }


        public async Task<List<SlideShow>> Fetch(string category)
        {
            return await _context.SlideShows.Where(p=> category!=null?p.Category.ToUpper() == category.ToUpper():true).ToListAsync();
        }

        public async Task<Response> Update(UpdSlideShowModel model)
        {
            if (String.IsNullOrEmpty(model.Category))
                return new Response { Status = "error", Message = "Invalid model!" };
            var slide = await _context.SlideShows.FindAsync(model.Id);
            if (slide != null)
            {
                if(model.Image!=null)
                {
                    string image = await UploadImage(slide.Id, model.Image,slide.Image);
                    slide.Image = "Resources/Images/SlideShows/" + slide.Id + "/" + image;
                }
                if (model.ImageMobile != null)
                {
                    string imageMobile = await UploadImage(slide.Id, model.ImageMobile,slide.ImageMobile);
                    slide.ImageMobile = "Resources/Images/SlideShows/" + slide.Id + "/" + imageMobile;
                }

                slide.Category = model.Category;
                
                await Save();
                return new Response { Status = "success", Message = "Слайд успешно изменена!" };
            }
            return new Response { Status = "error", Message = "Слайд не существует!" };
        }

        public async Task<Response> Delete(int id)
        {
            var slide = await _context.SlideShows.FindAsync(id);
            if (slide != null)
            {
                _context.SlideShows.Remove(slide);
                await Save();
                string path = Path.GetFullPath("Resources/Images/SlideShows/" + slide.Id);
                if (Directory.Exists(path))
                {
                    Directory.Delete(path,true);
                }
                return new Response { Status = "success", Message = "Слайд успешно удалена!" };
            }
            else
            {
                return new Response { Status = "error", Message = "Слайд не существует!" };
            }
        }


        private async Task<int> Save()
        {
            return await _context.SaveChangesAsync();
        }


        private async Task<string> UploadImage(int slideId, IFormFile file,string _path)
        {
            string path = Path.GetFullPath("Resources/Images/SlideShows/" + slideId);
            if (File.Exists(_path))
            {
                File.Delete(_path);
            }

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            path = Path.Combine(path, file.FileName);
            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            return file.FileName;
        }

    }
}