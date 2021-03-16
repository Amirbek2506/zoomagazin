using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using ZooMag.Data;
using ZooMag.Models.Entity;
using ZooMag.Models.ViewModels.Articles;
using ZooMag.Services.Interfaces;
using ZooMag.ViewModels;

namespace ZooMag.Services
{
    public class ArticlesService : IArticlesService
    {
        private readonly ApplicationDbContext _context;


        public ArticlesService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Response> Create(InpArticleModel article)
        {
            Article arl = new Article
            {
               Title = article.Title,
               ShortDescription = article.ShortDescription,
               Description = article.Description,
               CreatedAt = DateTime.Now,
               Image = "Resources/Images/Articles/default.jpg"
            };
            _context.Articles.Add(arl);
            if (await Save() > 0 && article.Image != null)
            {
                arl.Image = await UploadImage(arl.Id, article.Image);
                await Save();
            }
            return new Response {Status = "success", Message = "Успешно добавлен!"};
        }

        private async Task<string> UploadImage(int id, IFormFile file)
        {
            string path = Path.GetFullPath("Resources/Images/Articles/" + id);
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
            return "Resources/Images/Articles/" + id +"/"+ file.FileName;
        }


        public async Task<List<Article>> Get()
        {
            return await _context.Articles.ToListAsync();
        }

        public async Task<Article> GetById(int id)
        {
            return await _context.Articles.FindAsync(id);
        }


        public async Task<Response> Update(UpdArticleModel model)
        {
            var article = await _context.Articles.FindAsync(model.Id);
            if (article == null)
            {
                return new Response { Status = "error", Message = "Не найден!" };
            }
            article.Title = model.Title;
            article.ShortDescription = model.ShortDescription;
            article.Description = model.Description;
            if (model.Image != null)
            {
                article.Image = await UploadImage(article.Id, model.Image);
            }
            await Save();
            return new Response {Status = "success",Message = "Успешно изменен!"};
        }

        public async Task<Response> Delete(int id)
        {
            Article article = await _context.Articles.FindAsync(id);
            if(article==null)
            {
                return new Response { Status = "error", Message = "Не найден!" };
            }
            _context.Articles.Remove(article);
            await Save();
            return new Response {Status = "success",Message = "Успешно удален!" };
        }


        private async Task<int> Save()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
