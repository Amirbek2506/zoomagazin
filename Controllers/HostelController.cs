using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZooMag.Models.Entity;
using ZooMag.Models.ViewModels.Animals;
using ZooMag.Models.ViewModels.Articles;
using ZooMag.Models.ViewModels.Hostel;
using ZooMag.Models.ViewModels.PetOrders;
using ZooMag.Models.ViewModels.PetTransports;
using ZooMag.Services.Interfaces;
using ZooMag.ViewModels;

namespace ZooMag.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HostelController : ControllerBase
    {
        private readonly IHostelService _hostelService;

        public HostelController(IHostelService hostelService)
        {
            this._hostelService = hostelService;
        }

        #region Box

        [HttpPost]
        [Route("createbox")]
        [Authorize(Roles = "Администратор")]
        public async Task<IActionResult> CreateBox([FromForm] BoxModel model)
        {
            var ress = await _hostelService.CreateBox(model);
            if (ress.Status == "success")
            {
                return Ok(ress);
            }
            return BadRequest(ress);
        }

        [HttpGet]
        [Route("fetchboxes")]
        public async Task<IActionResult> GetBoxes()
        {
            return Ok(await _hostelService.GetBoxes());
        }

        [HttpGet]
        [Route("fetchfreeboxes")]
        public async Task<IActionResult> GetFreeBoxes()
        {
            return Ok(await _hostelService.GetFreeBoxes());
        }

        [HttpPost]
        [Route("changestatusbox")]
        [Authorize(Roles = "Администратор")]
        public async Task<IActionResult> ChangeStatusBox(int id, string status)
        {
            var ress = await _hostelService.ChangeStatusBox(id,status);
            if (ress.Status == "success")
            {
                return Ok(ress);
            }
            return BadRequest(ress);
        }

        [HttpDelete]
        [Route("deletebox/{id}")]
        [Authorize(Roles = "Администратор")]
        public async Task<IActionResult> DeleteBox(int id)
        {
            var ress = await _hostelService.DeleteBox(id);
            if (ress.Status == "success")
            {
                return Ok(ress);
            }
            return BadRequest(ress);
        }

        #endregion

        #region Order

        [HttpPost]
        [Route("createorder")]
        [Authorize(Roles = "Администратор")]
        public async Task<IActionResult> CreateOrder([FromForm] BoxOrderModel model)
        {
            var ress = await _hostelService.CreateOrder(model, int.Parse(User.Claims.First(i => i.Type == "UserId").Value));
            if (ress.Status == "success")
            {
                return Ok(ress);
            }
            return BadRequest(ress);
        }

        [HttpGet]
        [Route("fetchorders")]
        [Authorize(Roles = "Администратор")]
        public async Task<IActionResult> GetBoxOrders()
        {
            return Ok(await _hostelService.GetBoxOrders());
        }

        [HttpPost]
        [Route("changestatusorder")]
        [Authorize(Roles = "Администратор")]
        public async Task<IActionResult> ChangeStatusBoxOrder(int id, string status)
        {
            var ress = await _hostelService.ChangeStatusBoxOrder(id, status);
            if (ress.Status == "success")
            {
                return Ok(ress);
            }
            return BadRequest(ress);
        }


        [HttpPost]
        [Route("setboxtoorder")]
        [Authorize(Roles = "Администратор")]
        public async Task<IActionResult> SetBoxToOrder(int orderid, int boxid)
        {
            var ress = await _hostelService.SetBoxToOrder(orderid, boxid);
            if (ress.Status == "success")
            {
                return Ok(ress);
            }
            return BadRequest(ress);
        }


        [HttpDelete]
        [Route("deleteorder/{id}")]
        [Authorize(Roles = "Администратор")]
        public async Task<IActionResult> DeleteBoxOrder(int id)
        {
            var ress = await _hostelService.DeleteBoxOrder(id);
            if (ress.Status == "success")
            {
                return Ok(ress);
            }
            return BadRequest(ress);
        }

        #endregion

        #region BoxType

        [HttpPost]
        [Route("createtype")]
        [Authorize(Roles = "Администратор")]
        public async Task<IActionResult> CreateBoxType([FromForm] BoxTypeModel model)
        {
            var ress = await _hostelService.CreateBoxType(model);
            if (ress.Status == "success")
            {
                return Ok(ress);
            }
            return BadRequest(ress);
        }

        [HttpGet]
        [Route("fetchtypes")]
        public async Task<IActionResult> GetBoxTypes()
        {
            return Ok(await _hostelService.GetBoxTypes());
        }
        /*
        [HttpDelete]
        [Route("deletetype/{id}")]
        [Authorize(Roles = "Администратор")]
        public async Task<IActionResult> DeleteBoxType(int id)
        {
            var ress = await _hostelService.DeleteBoxType(id);
            if (ress.Status == "success")
            {
                return Ok(ress);
            }
            return BadRequest(ress);
        }*/

        #endregion
    }
}
