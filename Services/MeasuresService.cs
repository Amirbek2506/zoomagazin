using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using ZooMag.Data;
using ZooMag.Models;
using ZooMag.Models.ViewModels.Measures;
using ZooMag.Services.Interfaces;
using ZooMag.ViewModels;

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

        public async Task<Response> Update(Measure measure)
        {
            if(String.IsNullOrEmpty(measure.TitleRu) || String.IsNullOrEmpty(measure.TitleEn))
            {
                return new Response { Status = "error", Message = "Invalid Measure!" };
            }
            var _measure = await _context.Measures.FindAsync(measure.Id);
            if (_measure != null)
            {
                _measure.TitleRu = measure.TitleRu;
                _measure.TitleEn = measure.TitleEn;
                await Save();
                return new Response { Status = "success", Message = "Измерение успешно изменено!" };
            }
            else
            {
                return new Response { Status = "error", Message = "Измерение не найдено!" };
            }

        }

        public async Task<Response> Delete(int id)
        {
            var measure =await _context.Measures.FindAsync(id);
            if (measure != null)
            {
                _context.Measures.Remove(measure);
                await Save();
                return new Response { Status = "success", Message = "Измерения успешно удалено!" };
            }
            return new Response { Status = "error", Message = "Измерение не найдено!" };
        }

        public async Task<Response> Create(InpMeasureModel measureModel)
        {
            if (String.IsNullOrEmpty(measureModel.TitleRu) || String.IsNullOrEmpty(measureModel.TitleEn))
                return new Response { Status = "error", Message = "Invalid Category!" };
            _context.Measures.Add(
                new Measure 
                { 
                    TitleRu = measureModel.TitleRu,
                    TitleEn = measureModel.TitleEn
                });
            await Save();
            return new Response { Status = "success", Message = "Измерение успешно добавлено!" };
        }

        public async Task<int> Save()
        {
            return await _context.SaveChangesAsync();
        }
    }
}