using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using ZooMag.Entities;
using ZooMag.Models;
using ZooMag.Models.ViewModels.SlideShows;
using ZooMag.ViewModels;

namespace ZooMag.Services.Interfaces
{
    public interface ISlideShowsService
    {
        Task<Response> Create(InpSlideShowModel model);
        Task<List<SlideShow>> Fetch(string category);
        Task<Response> Update(UpdSlideShowModel model);
        Task<Response> Delete(int id);
    }
}