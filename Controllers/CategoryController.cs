using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ZooMag.Models;
using ZooMag.Models.ViewModels.Categories;
using ZooMag.Services.Interfaces;
using ZooMag.ViewModels;

namespace ZooMag.Controllers
{
    [ApiController]
    [Route("/")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoriesService _categoriesService;

        public CategoryController(ICategoriesService categoriesService)
        {
            this._categoriesService = categoriesService;
        }

        [HttpGet]
        [Route("Categories/fetch")]
        public async Task<IActionResult> GetCategories(bool hierarchie = true)
        {
            if(hierarchie)
            {
                return Ok(await _categoriesService.FetchWithSubcategories());
            }
            return Ok(await _categoriesService.Fetch());
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

        [HttpPost]
        [Route("Category/create")]
        [Authorize(Roles = "Администратор")]
        public async Task<IActionResult> CreateCategory([FromForm] InpCategoryModel categoryModel)
        {
            var ress = await _categoriesService.Create(categoryModel);
            if(ress.Status == "success")
            {
                return Ok(ress);
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
                return Ok(ress);
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
                return Ok(ress);
            }
            return BadRequest(ress);
        }

    }
}
