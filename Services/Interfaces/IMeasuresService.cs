using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using ZooMag.Models;

namespace ZooMag.Services.Interfaces
{
    public interface IMeasuresService
    {
        void Create(string title);
        int Count();
        void Delete(int id);
        Measure FetchById(int id);
        Task<List<Measure>> Fetch();
        void Update(int id, string title);
        Task<int> Save();
    }
}