using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using ZooMag.Models;
using ZooMag.Models.ViewModels.Measures;
using ZooMag.ViewModels;

namespace ZooMag.Services.Interfaces
{
    public interface IMeasuresService
    {
        Task<Response> Create(InpMeasureModel measureModel);
        int Count();
        Task<Response> Delete(int id);
        Measure FetchById(int id);
        Task<List<Measure>> Fetch();
        Task<Response> Update(Measure measure);
        Task<int> Save();
    }
}