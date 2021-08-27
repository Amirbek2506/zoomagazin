using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ZooMag.DTOs;
using ZooMag.DTOs.Brand;
using ZooMag.DTOs.FilterCategory;
using ZooMag.DTOs.SpecificFilter;
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
            return Created("Brand",response);
        }

        [HttpGet]
        public async Task<IActionResult> GetBrandSpecificFilters(int brandId)
        {
            List<SpecificFilterResponse> response = await _brandsService.GetBrandSpecificFiltersAsync(brandId);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetBrandFilters(int brandId)
        {
            List<FilterCategoryResponse> response = await _brandsService.GetBrandFiltersAsync(brandId);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAndOrderingByFirstCharacter()
        {
            List<AlphabetCharacterWithBrandsResponse> response =
                await _brandsService.GetAllAndOrderingByFirstCharacterAsync();
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
            return Created("Brand", response);
        }

        [HttpDelete]
        [Authorize(Roles = "Администратор")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _brandsService.DeleteAsync(id);
            return Created("Brand",response);
        }
    }
}
