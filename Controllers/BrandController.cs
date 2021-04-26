using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ZooMag.Models;
using ZooMag.Models.ViewModels.Brands;
using ZooMag.Models.ViewModels.Categories;
using ZooMag.Services.Interfaces;
using ZooMag.ViewModels;

namespace ZooMag.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BrandController : ControllerBase
    {
        private readonly IBrandsService _brandsService;

        public BrandController(IBrandsService brandsService)
        {
            this._brandsService = brandsService;
        }

        [HttpGet]
        [Route("fetch")]
        public async Task<IActionResult> GetBrands()
        {
            return Ok(await _brandsService.Fetch());
        }


        [HttpGet]
        [Route("fetchbyid/{id}")]
        public IActionResult GetById(int id)
        {
            var brand = _brandsService.FetchById(id);
            if (brand == null)
            {
                return BadRequest(new Response { Status = "Error", Message = "Бренд не найдена!" });
            }
            return Ok(brand);
        }

        [HttpPost]
        [Route("create")]
        [Authorize(Roles = "Администратор")]
        public async Task<IActionResult> Create([FromForm] InpBrandModel model)
        {
            var ress = await _brandsService.Create(model);
            if(ress.Status == "success")
            {
                return Ok(ress);
            }
            return BadRequest(ress);
        }
        
        [HttpPut]
        [Route("update")]
        [Authorize(Roles = "Администратор")]
        public async Task<IActionResult> Update([FromForm] UpdBrandModel model)
        {
            Response ress = await _brandsService.Update(model);
            if (ress.Status == "success")
            {
                return Ok(ress);
            }
            return BadRequest(ress);
        }

        [HttpDelete]
        [Route("delete")]
        [Authorize(Roles = "Администратор")]
        public async Task<IActionResult> Delete([FromForm] int id)
        {
             Response ress = await _brandsService.Delete(id);
            if (ress.Status == "success")
            {
                return Ok(ress);
            }
            return BadRequest(ress);
        }

    }
}
