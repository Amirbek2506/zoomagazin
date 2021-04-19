using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZooMag.Models.Entity;
using ZooMag.Models.ViewModels.Animals;
using ZooMag.Services.Interfaces;
using ZooMag.ViewModels;

namespace ZooMag.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AnimalController : ControllerBase
    {
        private readonly IAnimalsService _animalsService;

        public AnimalController(IAnimalsService animalsService)
        {
            this._animalsService = animalsService;
        }


        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Create([FromForm] InpAnimalModel animal)
        {
            var ress = await _animalsService.Create(animal, int.Parse(User.Claims.First(i => i.Type == "UserId").Value));
            if (ress.Status == "success")
            {
                return Ok(ress);
            }
            return BadRequest(ress);
        }

        [HttpGet]
        [Route("fetchmy")]
        public async Task<IActionResult> FetchMyAnimals()
        {
            return Ok(await _animalsService.GetMyAnimals(int.Parse(User.Claims.First(i => i.Type == "UserId").Value)));
        }
        
        [HttpGet]
        [Route("fetch")]
        public async Task<IActionResult> FetchAnimals(int typeid=0)
        {
            return Ok(await _animalsService.GetAnimals(typeid, int.Parse(User.Claims.First(i => i.Type == "UserId").Value)));
        }

        [HttpGet]
        [Route("fetchbyid/{id}")]
        public async Task<IActionResult> FetchAnimalById(int id)
        {
            var animal = await _animalsService.GetAnimalById(id);
            if(animal!=null)
            {
                return Ok(animal);
            }
            return BadRequest(new Response {Status = "error",Message = "Не нейден!"});
        }
        
        [HttpPut]
        [Route("update")]
        public async Task<IActionResult> UpdateAnimal([FromForm]UpdAnimalModel model)
        {
            var ress = await _animalsService.UpdateAnimal(model, int.Parse(User.Claims.First(i => i.Type == "UserId").Value));
            if(ress.Status == "success")
            {
                return Ok(ress);
            }
            return BadRequest(ress);
        }

        [HttpDelete]
        [Route("delete/{id}")]
        public async Task<IActionResult> DeleteAnimal(int id)
        {
            var ress = await _animalsService.Delete(id, int.Parse(User.Claims.First(i => i.Type == "UserId").Value));
            if(ress.Status == "success")
            {
                return Ok(ress);
            }
            return BadRequest(ress);
        }

        [HttpGet]
        [Route("fetchgenders")]
        public async Task<IActionResult> FetchAnimalGenders()
        {
            return Ok(await _animalsService.GetAnimalGenders());
        }
        
        [HttpGet]
        [Route("fetchtypes")]
        public async Task<IActionResult> FetchAnimalTypes()
        {
            return Ok(await _animalsService.GetAnimalTypes());
        }

    }
}
