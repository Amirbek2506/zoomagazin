using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ZooMag.Models;
using ZooMag.Models.ViewModels.Carts;
using ZooMag.Services.Interfaces;
using ZooMag.ViewModels;

namespace ZooMag.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CartController : ControllerBase
    {

        private readonly ICartsService _cartsService;

        public CartController(ICartsService cartsService)
        {
            this._cartsService = cartsService;
        }
        /*
        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Create([FromForm] InpCartModel model)
        {
            string userKey = GetUserKey();
            CartModel cartModel = await _cartsService.Create(model, userKey);
            if(cartModel!=null)
            {
                return Ok(cartModel);
            }
            return BadRequest();
        }

        [HttpPost]
        [Route("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            string userKey = GetUserKey();
            Response ress = await _cartsService.Delete(id, userKey);
            if(ress.Status == "success")
            {
                return Ok(ress);
            }
            return BadRequest(ress);
        }

        [HttpGet]
        [Route("fetch")]
        public async Task<IActionResult> FetchCartItems()
        {
            string userKey = GetUserKey();
            return Ok(await _cartsService.FetchCartItems(userKey));
        }
        
        [HttpGet]
        [Route("count")]
        public async Task<IActionResult> Count()
        {
            string userKey = GetUserKey();
            return Ok(await _cartsService.Count(userKey));
        }

        [HttpPost]
        [Route("setsize")]
        public async Task<IActionResult> SetSize([FromForm]int cartid,[FromForm]int sizeid)
        {
            string userKey = GetUserKey();
            Response ress = await _cartsService.SetSize(cartid, sizeid, userKey);
            if (ress.Status == "success")
            {
                return Ok(ress);
            }
            return BadRequest(ress);
        }

        [HttpPost]
        [Route("incrqty/{id}")]
        public async Task<IActionResult> IncrQty(int id)
        {
            string userKey = GetUserKey();
            var ress = await _cartsService.IncrQty(id, userKey);
            if (ress != 0)
            {
                return Ok(ress);
            }
            return BadRequest();
        }

        [HttpPost]
        [Route("decrqty/{id}")]
        public async Task<IActionResult> DecrQty(int id)
        {
            string userKey = GetUserKey();
            var ress = await _cartsService.DecrQty(id, userKey);
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
                if (Request.Cookies["cartid"] == null)
                {
                    CookieOptions cookieOptions = new CookieOptions();
                    cookieOptions.Expires = DateTime.Now.AddMonths(2);
                    // Generate a new random GUID using System.Guid class.     
                    Guid tempCartId = Guid.NewGuid();
                    Response.Cookies.Append("cartid", tempCartId.ToString(), cookieOptions);
                    return tempCartId.ToString();
                }
                else
                {
                    CookieOptions cookieOptions = new CookieOptions();
                    cookieOptions.Expires = DateTime.Now.AddMonths(2);
                    Response.Cookies.Append("cartid", Request.Cookies["cartid"].ToString(), cookieOptions);
                    return Request.Cookies["cartid"].ToString();
                }
            }
        }*/
    }
}
