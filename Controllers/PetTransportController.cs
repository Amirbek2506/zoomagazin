using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZooMag.Models.Entity;
using ZooMag.Models.ViewModels.Animals;
using ZooMag.Models.ViewModels.Articles;
using ZooMag.Models.ViewModels.PetTransports;
using ZooMag.Services.Interfaces;
using ZooMag.ViewModels;

namespace ZooMag.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PetTransportController : ControllerBase
    {
        private readonly IPetTransportsService _petTransportsService;

        public PetTransportController(IPetTransportsService petTransportsService)
        {
            this._petTransportsService = petTransportsService;
        }


        [HttpPost]
        [Route("create")]
        [Authorize]
        public async Task<IActionResult> Create([FromForm] InpPetTransportModel model)
        {
            var ress = await _petTransportsService.Create(model, int.Parse(User.Claims.First(i => i.Type == "UserId").Value));
            if (ress.Status == "success")
            {
                return Ok(ress);
            }
            return BadRequest(ress);
        }

        
        [HttpGet]
        [Route("fetch")]
        [Authorize(Roles = "Администратор")]
        public async Task<IActionResult> Fetch()
        {
            return Ok(await _petTransportsService.Get());
        }

        [HttpGet]
        [Route("fetchbyid/{id}")]
        [Authorize(Roles = "Администратор")]
        public async Task<IActionResult> FetchById(int id)
        {
            var petTransport = await _petTransportsService.GetById(id);
            if(petTransport != null)
            {
                return Ok(petTransport);
            }
            return BadRequest(new Response {Status = "error",Message = "Не нейден!"});
        }
        
        [HttpPut]
        [Route("changestatus")]
        [Authorize(Roles = "Администратор")]
        public async Task<IActionResult> ChangeStatus(int id,int statusid)
        {
            var ress = await _petTransportsService.ChangeStatus(id,statusid);
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
            var ress = await _petTransportsService.Delete(id);
            if(ress.Status == "success")
            {
                return Ok(ress);
            }
            return BadRequest(ress);
        }
    }
}
