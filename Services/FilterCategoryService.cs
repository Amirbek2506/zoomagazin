using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ZooMag.Data;
using ZooMag.DTOs.Filter;
using ZooMag.DTOs.FilterCategory;
using ZooMag.Entities;
using ZooMag.Services.Interfaces;
using ZooMag.ViewModels;

namespace ZooMag.Services
{
    public class FilterCategoryService : IFilterCategoryService
    {
        private readonly ApplicationDbContext _context;

        public FilterCategoryService(ApplicationDbContext context)
        {
            _context = context;
        }
        
        public async Task<List<FilterCategoryResponse>> GetAllAsync()
        {
            return await _context.FilterCategories.Include(x => x.Filters).Select(x => new FilterCategoryResponse
            {
                Text = x.Text,
                Filters = x.Filters.Select(f => new FilterResponse
                {
                    Id = f.Id,
                    Text = f.Text
                }).ToList()
            }).ToListAsync();
        }

        public async Task<Response> CreateAsync(CreateFilterCategoryRequest request)
        {
            var filterCategory = new FilterCategory
            {
                Text = request.Text
            };

            await _context.FilterCategories.AddAsync(filterCategory);
            await _context.SaveChangesAsync();
            
            return new()
            {
                Status = "success",
                Message = "Успешно"
            };
        }
    }
}