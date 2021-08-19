using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZooMag.Entities;
using ZooMag.Models.ViewModels.Categories;
using ZooMag.Models.ViewModels.PetCategories;
using ZooMag.Services.Interfaces;
using ZooMag.ViewModels;

namespace ZooMag.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PetCategoryController : ControllerBase
    {
        private readonly IPetCategoriesService _categoriesService;

        public PetCategoryController(IPetCategoriesService categoriesService)
        {
            this._categoriesService = categoriesService;
        }

        [HttpGet]
        [Route("fetch")]
        public async Task<IActionResult> GetPetCategories(bool hierarchie = true)
        {
            if(hierarchie)
            {
                return Ok(await _categoriesService.FetchWithSubcategories());
            }
            return Ok(await _categoriesService.Fetch());
        }


        [HttpGet]
        [Route("fetchbyid/{id}")]
        public IActionResult GetPetCategoryById(int id)
        {
            PetCategory category = _categoriesService.FetchById(id);
            if (category == null)
            {
                return BadRequest(new Response { Status = "Error", Message = "Категория не найдена!" });
            }
            return Ok(category);
        }

        [HttpPost]
        [Route("create")]
        //[Authorize(Roles = "Администратор")]
        public async Task<IActionResult> CreatePetCategory([FromForm] InpPetCategoryModel categoryModel)
        {
            var ress = await _categoriesService.Create(categoryModel);
            if(ress.Status == "success")
            {
                return Ok(ress);
            }
            return BadRequest(ress);
        }
        
        [HttpPut]
        [Route("update")]
        //[Authorize(Roles = "Администратор")]
        public async Task<IActionResult> UpdatePetCategory([FromForm] UpdPetCategoryModel categoryModel)
        {
            Response ress = await _categoriesService.Update(categoryModel);
            if (ress.Status == "success")
            {
                return Ok(ress);
            }
            return BadRequest(ress);
        }

        [HttpDelete]
        [Route("delete")]
        //[Authorize(Roles = "Администратор")]
        public async Task<IActionResult> DeletePetCategory([FromForm] int id)
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
