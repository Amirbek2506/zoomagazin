using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ZooMag.Models;
using ZooMag.Models.ViewModels.Categories;
using ZooMag.Models.ViewModels.SlideShows;
using ZooMag.Services.Interfaces;
using ZooMag.ViewModels;

namespace ZooMag.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SlideShowController : ControllerBase
    {
        private readonly ISlideShowsService _slideShowsService;

        public SlideShowController(ISlideShowsService slideShowsService)
        {
            this._slideShowsService = slideShowsService;
        }

        [HttpGet]
        [Route("fetch")]
        public async Task<IActionResult> GetSlideShows(string category)
        {
            return Ok(await _slideShowsService.Fetch(category));
        }



        [HttpPost]
        [Route("create")]
        [Authorize(Roles = "Администратор")]
        public async Task<IActionResult> CreateSlideShow([FromForm] InpSlideShowModel model)
        {
            var ress = await _slideShowsService.Create(model);
            if(ress.Status == "success")
            {
                return Ok(ress);
            }
            return BadRequest(ress);
        }
        
        [HttpPut]
        [Route("update")]
        [Authorize(Roles = "Администратор")]
        public async Task<IActionResult> UpdateCategory([FromForm] UpdSlideShowModel model)
        {
            Response ress = await _slideShowsService.Update(model);
            if (ress.Status == "success")
            {
                return Ok(ress);
            }
            return BadRequest(ress);
        }

        [HttpDelete]
        [Route("delete")]
        [Authorize(Roles = "Администратор")]
        public async Task<IActionResult> DeleteSlideShow([FromForm] int id)
        {
             Response ress = await _slideShowsService.Delete(id);
            if (ress.Status == "success")
            {
                return Ok(ress);
            }
            return BadRequest(ress);
        }

    }
}
