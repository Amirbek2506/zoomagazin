using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ZooMag.Models.ViewModels.Orders;
using ZooMag.Services.Interfaces;

namespace ZooMag.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController : ControllerBase
    {

        private readonly IOrdersService _ordersService;

        public OrderController(IOrdersService ordersService)
        {
            this._ordersService = ordersService;
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Create([FromBody] InpOrderModel model)
        {
            string userKey = GetUserKey();
            var ress = await _ordersService.Create(model, userKey);
            if (ress.Status == "success")
            {
                return Ok(ress);
            }
            return BadRequest(ress);
        }

        [HttpGet]
        [Route("/fetchmyorders")]
        public async Task<IActionResult> FetchMyOrders()
        {
            string userKey = GetUserKey();
            if(userKey=="0")
            {
                return BadRequest();
            }
            return Ok(await _ordersService.FetchMyOrders(userKey));
        }
        
        [HttpGet]
        [Route("fetchdetail/{orderid}")]
        public async Task<IActionResult> FetchDetail(int orderid)
        {
            var ress = await _ordersService.FetchDetail(orderid);
            if(ress!=null)
            {
                return Ok(ress);
            }
            return BadRequest();
        }

        [HttpGet]
        [Route("fetch")]
        [Authorize(Roles = "Администратор")]
        public async Task<IActionResult> FetchAll(int offset = 0, int limit = 20)
        {
            return Ok(new { count = await _ordersService.Count(),orders = await _ordersService.FetchAll(offset, limit) });
        }

        
        [HttpPost]
        [Route("setsize")]
        [Authorize(Roles = "Администратор")]
        public async Task<IActionResult> SetSize(int orderitemid, int sizeid)
        {
            var ress = await _ordersService.SetSize(orderitemid,sizeid);
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
            var ress = await _ordersService.Delete(id);
            if(ress.Status=="success")
            {
                return Ok(ress);
            }
            return BadRequest(ress);
        }
        
        [HttpDelete]
        [Route("deleteitem/{id}")]
        [Authorize(Roles = "Администратор")]
        public async Task<IActionResult> DeleteItem(int id)
        {
            var ress = await _ordersService.DeleteItem(id);
            if(ress.Status=="success")
            {
                return Ok(ress);
            }
            return BadRequest(ress);
        }


        [HttpPost]
        [Route("incrqty/{itemid}")]
        [Authorize(Roles = "Администратор")]
        public async Task<IActionResult> IncrQty(int itemid)
        {
            var ress = await _ordersService.IncrQty(itemid);
            if (ress != 0)
            {
                return Ok(ress);
            }
            return BadRequest();
        }

        [HttpPost]
        [Route("decrqty/{itemid}")]
        [Authorize(Roles = "Администратор")]
        public async Task<IActionResult> DecrQty(int itemid)
        {
            var ress = await _ordersService.DecrQty(itemid);
            if (ress != 0)
            {
                return Ok(ress);
            }
            return BadRequest();
        }


        private string GetUserKey()
        {
            if (User.Identity.IsAuthenticated)
            {
                return User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
            }
            else
            {
                return "0";
            }
        }
    }
}
