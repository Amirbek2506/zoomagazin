using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZooMag.DTOs;
using ZooMag.DTOs.Banner;
using ZooMag.Services.Interfaces;
using ZooMag.ViewModels;

namespace ZooMag.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class BannerController : ControllerBase
    {
        private readonly IBannerService _bannerService;

        public BannerController(IBannerService bannerService)
        {
            _bannerService = bannerService;
        }

        [HttpPost]
        [Authorize(Roles = "Администратор")]
        public async Task<IActionResult> Create([FromForm]CreateBannerRequest request)
        {
            Response response = await _bannerService.CreateAsync(request);
            return Created("Banner", response);
        }

        [HttpPut]
        [Authorize(Roles = "Администратор")]
        public async Task<IActionResult> Update([FromForm]UpdateBannerRequest request)
        {
            Response response = await _bannerService.UpdateAsync(request);
            return Created("Banner", response);
        }

        [HttpDelete]
        [Authorize(Roles = "Администратор")]
        public async Task<IActionResult> Delete([FromQuery]int id)
        {
            Response response = await _bannerService.DeleteAsync(id);
            return Created("Banner", response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery]PagedRequest request)
        {
            List<BannerResponse> response = await _bannerService.GetAllAsync(request);
            return Ok(response);
        }
    }
}