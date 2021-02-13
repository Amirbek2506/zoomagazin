using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ZooMag.Models;
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
        public async Task<IActionResult> GetCategories([FromForm] bool hierarchie = true)
        {
            if(hierarchie)
            {
                return Ok(await _categoriesService.FetchWithSubcategories());
            }
            return Ok(await _categoriesService.Fetch());
        }


        [HttpGet]
        [Route("Category/fetchbyid")]
        public IActionResult GetCategoryById([FromForm] int id)
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
        public async Task<IActionResult> CreateCategory([FromForm] int parentid, [FromForm]string title)
        {
            if (String.IsNullOrEmpty(title))
                return BadRequest(new Response { Status = "error", Message = "Invalid Category!" });
            _categoriesService.Create(parentid,title);
            await _categoriesService.Save();
            return Ok(new Response { Status = "success", Message = "Категория успешно добавлена!" });
        }
        
        [HttpPut]
        [Route("Category/update")]
        [Authorize(Roles = "Администратор")]
        public async Task<IActionResult> UpdateCategory([FromForm] int id, [FromForm] string title)
        {
            if (String.IsNullOrEmpty(title))
                return StatusCode(StatusCodes.Status400BadRequest, new Response { Status = "error", Message = "Invalid Category!" });
            Response ress = await _categoriesService.Update(id, title);
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
