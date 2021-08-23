using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ZooMag.Data;
using ZooMag.DTOs.SpecificFilter;
using ZooMag.Entities;
using ZooMag.Services.Interfaces;
using ZooMag.ViewModels;

namespace ZooMag.Services
{
    public class SpecificFilterService : ISpecificFilterService
    {
        private readonly ApplicationDbContext _context;

        public SpecificFilterService(ApplicationDbContext context)
        {
            _context = context;
        }
        
        public async Task<Response> CreateAsync(CreateSpecificFilterRequest request)
        {
            var specificFilter = new SpecificFilter
            {
                Text = request.Text
            };

            if (request.ProductIds.Count > 0)
                specificFilter.ProductSpecificFilters = request.ProductIds.Select(x => new ProductSpecificFilter
                {
                    ProductId = x
                }).ToList();

            await _context.SpecificFilters.AddAsync(specificFilter);
            await _context.SaveChangesAsync();
            return new()
            {
                Status = "success",
                Message = "Успешно"
            };
        }

        public async Task<List<SpecificFilterResponse>> GetAllAsync()
        {
            return await _context.SpecificFilters.Select(x => new SpecificFilterResponse
            {
                Id = x.Id,
                Text = x.Text
            }).ToListAsync();
        }
    }
}