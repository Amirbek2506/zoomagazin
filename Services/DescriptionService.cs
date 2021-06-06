using System.Threading.Tasks;
using ZooMag.Data;
using ZooMag.DTOs.Description;
using ZooMag.Entities;
using ZooMag.Services.Interfaces;
using ZooMag.ViewModels;

namespace ZooMag.Services
{
    public class DescriptionService : IDescriptionService
    {
        private readonly ApplicationDbContext _context;

        public DescriptionService(ApplicationDbContext context)
        {
            _context = context;
        }
        
        public async Task<Response> CreateAsync(CreateDescriptionRequest request)
        {
            if (request.ProductItemId != null)
            {
                var description = new Description
                {
                    Content = request.Content,
                    Title = request.Title,
                    ProductItemId = (int) request.ProductItemId
                };

                await _context.Descriptions.AddAsync(description);
                await _context.SaveChangesAsync();

                return new Response {Status = "success", Message = "Успешно"};
            }

            return new Response {Message = "Неверно заполнены поля", Status = "error"};
        }

        public async Task<Response> UpdateAsync(UpdateDescriptionRequest request)
        {
            var description = await _context.Descriptions.FindAsync(request.Id);

            if (description == null)
                return new Response {Message = "Не найден", Status = "error"};

            description.Content = request.Content;
            description.Title = request.Title;

            await _context.SaveChangesAsync();
            
            return new Response {Status = "success", Message = "Успешно"};
        }

        public async Task<Response> DeleteAsync(int id)
        {
            var description = await _context.Descriptions.FindAsync(id);
            
            if (description == null)
                return new Response {Message = "Не найден", Status = "error"};

            _context.Descriptions.Remove(description);

            await _context.SaveChangesAsync();

            return new Response {Status = "success", Message = "Успешно"};
        }
    }
}