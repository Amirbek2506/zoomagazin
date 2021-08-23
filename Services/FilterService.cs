using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ZooMag.Data;
using ZooMag.DTOs.Filter;
using ZooMag.Entities;
using ZooMag.Services.Interfaces;
using ZooMag.ViewModels;

namespace ZooMag.Services
{
    public class FilterService : IFilterService
    {
        private readonly ApplicationDbContext _context;

        public FilterService(ApplicationDbContext context)
        {
            _context = context;
        }
        
        public async Task<Response> CreateAsync(CreateFilterRequest request)
        {
            var filter = new Filter
            {
                Text = request.Text,
                FilterCategoryId = request.FilterCategoryId
            };

            if (request.CategoryIds.Count > 0)
                filter.CategoryFilters = request.CategoryIds.Select(x => new CategoryFilter {CategoryId = x}).ToList();

            await _context.Filters.AddAsync(filter);
            await _context.SaveChangesAsync();

            return new()
            {
                Status = "success",
                Message = "Успешно"
            };
        }

        public async Task<List<FilterResponse>> GetAllAsync()
        {
            return await _context.Filters.Select(x => new FilterResponse {Id = x.Id, Text = x.Text}).ToListAsync();
        }

        public async Task<List<SelectOptionFilterResponse>> GetFilterForSelectOptionAsync()
        {
            return await _context.Filters.Select(x => new SelectOptionFilterResponse
            {
                Id = x.Id,
                Text = x.Text
            }).ToListAsync();
        }
    }
}