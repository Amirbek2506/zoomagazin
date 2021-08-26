using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZooMag.DTOs.FilterCategory;
using ZooMag.DTOs.SpecificFilter;
using ZooMag.Entities;
using ZooMag.Models.ViewModels.Categories;
using ZooMag.Services.Interfaces;
using ZooMag.ViewModels;

namespace ZooMag.Controllers
{
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoriesService _categoriesService;

        public CategoryController(ICategoriesService categoriesService)
        {
            this._categoriesService = categoriesService;
        }

        [HttpGet]
        [Route("Categories/GetCategorySpecificFilters")]
        public async Task<IActionResult> GetCategorySpecificFilters(int categoryId)
        {
            List<SpecificFilterResponse> response = await _categoriesService.GetCategorySpecificFiltersAsync(categoryId);
            return Ok(response);
        }

        [HttpGet]
        [Route("Categories/GetCategoryFilters")]
        public async Task<IActionResult> GetCategoryFilters(int categoryId)
        {
            var response = await _categoriesService.GetCategoryFilters(categoryId);
            return Ok(response);
        }

        [HttpGet]
        [Route("Categories/GetCategoriesForSelectOption")]
        public async Task<IActionResult> GetCategoriesForSelectOption()
        {
            var response = await _categoriesService.GetCategoriesForSelectOptionAsync();
            return Ok(response);
        }

        [HttpGet]
        [Route("Categories/fetch")]
        public async Task<IActionResult> GetCategories(bool hierarchie = true)
        {
            return hierarchie ? Ok(await _categoriesService.FetchWithSubcategories()) : Ok(await _categoriesService.Fetch());
        }


        [HttpGet]
        [Route("Category/fetchbyid/{id}")]
        public IActionResult GetCategoryById(int id)
        {
            Category category = _categoriesService.FetchById(id);
            if (category == null)
            {
                return BadRequest(new Response { Status = "Error", Message = "Категория не найдена!" });
            }
            return Ok(category);
        }

        [HttpGet]
        [Route("category/getcategorybrands/{id}")]
        public async Task<IActionResult> GetCategoryBrands(int id)
        {
            var response = await _categoriesService.GetCategoryBrands(id);
            return Ok(response); 
        }

        [HttpPost]
        [Route("Category/create")]
        [Authorize(Roles = "Администратор")]
        public async Task<IActionResult> CreateCategory([FromForm] InpCategoryModel categoryModel)
        {
            var ress = await _categoriesService.Create(categoryModel);
            if(ress.Status == "success")
            {
                return Created("Category",ress);
            }
            return BadRequest(ress);
        }
        
        [HttpPut]
        [Route("Category/update")]
        [Authorize(Roles = "Администратор")]
        public async Task<IActionResult> UpdateCategory([FromForm] UpdCategoryModel categoryModel)
        {
            Response ress = await _categoriesService.Update(categoryModel);
            if (ress.Status == "success")
            {
                return Created("Category",ress);
            }
            return BadRequest(ress);
        }

        [HttpDelete]
        [Route("Category/delete")]
        [Authorize(Roles = "Администратор")]
        public async Task<IActionResult> DeleteCategory([FromForm] int id)
        {
             Response ress = await _categoriesService.Delete(id);
            if (ress.Status == "success")
            {
                return Created("Category",ress);
            }
            return BadRequest(ress);
        }

    }
}
