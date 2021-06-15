using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ZooMag.Data;
using ZooMag.DTOs.Comment;
using ZooMag.Entities;
using ZooMag.Services.Interfaces;
using ZooMag.ViewModels;

namespace ZooMag.Services
{
    public class CommentService : ICommentService
    {
        private readonly ApplicationDbContext _context;

        public CommentService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<CommentResponse>> GetAllAsync()
        {
            return await _context.Comments.OrderByDescending(x => x.CommentDate).Select(x=> new CommentResponse
            {
                Id = x.Id,
                Rating = x.Rating,
                Text = x.Text,
                CommentDate = x.CommentDate,
                UserName = x.UserName,
                ProductItemId = x.ProductItemId
            }).ToListAsync();
        }

        public async Task<Response> DeleteAsync(int id)
        {
            var comment = await _context.Comments.FindAsync(id);
            if (comment == null)
                return new Response {Message = "Не найдено", Status = "error"};
            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();
            return new Response {Message = "Успешно", Status = "success"};
        }

        public async Task<List<CommentResponse>> GetProductItemReviewsAsync(int productItemId)
        {
            return await _context.Comments.Where(x => x.ProductItemId == productItemId)
                .Select(x => new CommentResponse
                {
                    Id = x.Id,
                    Rating = x.Rating,
                    Text = x.Text,
                    CommentDate = x.CommentDate,
                    UserName = x.UserName,
                    ProductItemId = x.ProductItemId
                }).ToListAsync();
        }

        public async Task<Response> CreateAsync(CreateCommentRequest request)
        {
            var review = new Comment
            {
                Text = request.Text,
                CommentDate = DateTime.Now,
                ProductItemId = request.ProductItemId,
                Rating = request.Rating,
                Removed = false,
                UserName = request.UserName
            };
            await _context.Comments.AddAsync(review);
            await _context.SaveChangesAsync();
            return new Response
            {
                Message = "Успешно",
                Status = "success"
            };
        }
    }
}