using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using ZooMag.Data;
using ZooMag.Models;
using ZooMag.Services.Interfaces;

namespace ZooMag.Services
{
    public class MeasuresService : IMeasuresService
    {
        private readonly ApplicationDbContext _context;

        public MeasuresService(ApplicationDbContext context)
        {
            _context = context;
        }

        public int Count()
        {
            return _context.Categories.Count();
        }

        public Measure FetchById(int id)
        {
            return _context.Measures.FirstOrDefault(c => c.Id == id);
        }

        public async Task<List<Measure>> Fetch()
        {
            return await _context.Measures.ToListAsync();
        }

        public void Update(int id, string title)
        {
            var measure = FetchById(id);
            if (measure != null)
            {
                measure.TitleRu = title;
                _context.Measures.Update(measure);
            }
        }

        public void Delete(int id)
        {
            var measure = FetchById(id);
            if (measure != null)
            {
                _context.Measures.Remove(measure);
            }
        }

        public void Create(string title)
        {
            _context.Measures.Add(new Measure { TitleRu = title });
        }

        public async Task<int> Save()
        {
            return await _context.SaveChangesAsync();
        }
    }
}