using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ZooMag.DTOs.Brand;
using ZooMag.Models;
using ZooMag.Models.ViewModels.Brands;
using ZooMag.Models.ViewModels.Categories;
using ZooMag.Services.Interfaces;
using ZooMag.ViewModels;

namespace ZooMag.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class BrandController : ControllerBase
    {
        private readonly IBrandsService _brandsService;

        public BrandController(IBrandsService brandsService)
        {
            _brandsService = brandsService;
        }

        [HttpPost]
        [Authorize(Roles = "Администратор")]
        public async Task<IActionResult> Create([FromForm]CreateBrandRequest request)
        {
            var response = await _brandsService.CreateAsync(request);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await _brandsService.GetAllAsync();
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllWithCategories()
        {
            var response = await _brandsService.GetAllWithCategoriesAsync();
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetById(int id)
        {
            var response = await _brandsService.GetByIdAsync(id);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetBrandCategories(int id)
        {
            var response = await _brandsService.GetBrandCategoriesAsync(id);
            return Ok(response);
        }

        [HttpPut]
        [Authorize(Roles = "Администратор")]
        public async Task<IActionResult> Update([FromForm]UpdateBrandRequest request)
        {
            var response = await _brandsService.UpdateAsync(request);
            return Ok(response);
        }

        [HttpDelete]
        [Authorize(Roles = "Администратор")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _brandsService.DeleteAsync(id);
            return Ok(response);
        }
    }
}
