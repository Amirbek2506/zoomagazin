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

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Create([FromForm] InpCartModel model)
        {
            string cartKey = GetCartKey();
            CartModel cartModel = await _cartsService.Create(model, cartKey);
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
            string cartKey = GetCartKey();
            Response ress = await _cartsService.Delete(id, cartKey);
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
            string cartid = GetCartKey();
            return Ok(await _cartsService.FetchCartItems(cartid));
        }
        
        [HttpGet]
        [Route("count")]
        public async Task<IActionResult> Count()
        {
            string cartid = GetCartKey();
            return Ok(await _cartsService.Count(cartid));
        }

        [HttpPost]
        [Route("setsize")]
        public async Task<IActionResult> SetSize([FromForm]int cartid,[FromForm]int sizeid)
        {
            string cartKey = GetCartKey();
            Response ress = await _cartsService.SetSize(cartid, sizeid, cartKey);
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
            string cartKey = GetCartKey();
            var ress = await _cartsService.IncrQty(id, cartKey);
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
            string cartKey = GetCartKey();
            var ress = await _cartsService.DecrQty(id, cartKey);
            if (ress != 0)
            {
                return Ok(ress);
            }
            return BadRequest();
        }




        private string GetCartKey()
        {
            if (User.Identity.IsAuthenticated)
            {
                if (Request.Cookies["cartid"] == null)
                {
                    CookieOptions cookieOptions = new CookieOptions();
                    cookieOptions.Expires = DateTime.Now.AddMonths(1);
                    Response.Cookies.Append("cartid", User.FindFirstValue(ClaimTypes.NameIdentifier), cookieOptions);
                    return User.FindFirstValue(ClaimTypes.NameIdentifier);
                }
                return User.FindFirstValue(ClaimTypes.NameIdentifier);
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
        }
    }
}
