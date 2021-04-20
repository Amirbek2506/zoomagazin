using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZooMag.Models.Entity;
using ZooMag.Models.ViewModels.Animals;
using ZooMag.Models.ViewModels.Articles;
using ZooMag.Models.ViewModels.PetOrders;
using ZooMag.Models.ViewModels.PetTransports;
using ZooMag.Services.Interfaces;
using ZooMag.ViewModels;

namespace ZooMag.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PetOrderController : ControllerBase
    {
        private readonly IPetOrdersService _petOrdersService;

        public PetOrderController(IPetOrdersService petOrdersService)
        {
            this._petOrdersService = petOrdersService;
        }


        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Create([FromForm]PetOrderModel model)
        {
            var ress = await _petOrdersService.Create(model);
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
            return Ok(await _petOrdersService.Get());
        }


        [HttpDelete]
        [Route("delete/{id}")]
        [Authorize(Roles = "Администратор")]
        public async Task<IActionResult> Delete(int id)
        {
            var ress = await _petOrdersService.Delete(id);
            if(ress.Status == "success")
            {
                return Ok(ress);
            }
            return BadRequest(ress);
        }
    }
}
