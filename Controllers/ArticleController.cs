using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZooMag.Models.Entity;
using ZooMag.Models.ViewModels.Animals;
using ZooMag.Models.ViewModels.Articles;
using ZooMag.Services.Interfaces;
using ZooMag.ViewModels;

namespace ZooMag.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ArticleController : ControllerBase
    {
        private readonly IArticlesService _articlesService;

        public ArticleController(IArticlesService articlesService)
        {
            this._articlesService = articlesService;
        }


        [HttpPost]
        [Route("create")]
        [Authorize(Roles = "Администратор")]
        public async Task<IActionResult> Create([FromForm] InpArticleModel article)
        {
            var ress = await _articlesService.Create(article);
            if (ress.Status == "success")
            {
                return Ok(ress);
            }
            return BadRequest(ress);
        }

        
        [HttpGet]
        [Route("fetch")]
        public async Task<IActionResult> FetchArticles()
        {
            return Ok(await _articlesService.Get());
        }

        [HttpGet]
        [Route("fetchbyid/{id}")]
        public async Task<IActionResult> FetchArticleById(int id)
        {
            var animal = await _articlesService.GetById(id);
            if(animal!=null)
            {
                return Ok(animal);
            }
            return BadRequest(new Response {Status = "error",Message = "Не нейден!"});
        }
        
        [HttpPut]
        [Route("update")]
        [Authorize(Roles = "Администратор")]
        public async Task<IActionResult> Update([FromForm]UpdArticleModel model)
        {
            var ress = await _articlesService.Update(model);
            if(ress.Status == "success")
            {
                return Ok(ress);
            }
            return BadRequest(ress);
        }


        [HttpDelete]
        [Route("delete/{id}")]
        [Authorize(Roles = "Администратор")]
        public async Task<IActionResult> Delete(int id)
        {
            var ress = await _articlesService.Delete(id);
            if(ress.Status == "success")
            {
                return Ok(ress);
            }
            return BadRequest(ress);
        }
    }
}
