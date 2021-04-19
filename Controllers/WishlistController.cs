using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ZooMag.Models;
using ZooMag.Models.ViewModels.Carts;
using ZooMag.Models.ViewModels.Wishlist;
using ZooMag.Services.Interfaces;
using ZooMag.ViewModels;

namespace ZooMag.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WishlistController : ControllerBase
    {

        private readonly IWishlistService _wishlistService;

        public WishlistController(IWishlistService wishlistService)
        {
            this._wishlistService = wishlistService;
        }

        [HttpPost]
        [Route("create/{productid}")]
        public async Task<IActionResult> Create(int productid)
        {
            string userKey = GetUserKey();
            WishlistModel wishlistModel = 
                await _wishlistService.Create(
                new Wishlist 
                {
                    ProductId = productid,
                    UserKey = userKey
                });
            if (wishlistModel != null)
            {
                return Ok(wishlistModel);
            }
            return BadRequest();
        }

        [HttpGet]
        [Route("fetch")]
        public async Task<IActionResult> FetchItems()
        {
            string userKey = GetUserKey();
            return Ok(await _wishlistService.FetchItems(userKey));
        }

        [HttpGet]
        [Route("count")]
        public async Task<IActionResult> Count()
        {
            string userKey = GetUserKey();
            return Ok(await _wishlistService.Count(userKey));
        }


        [HttpPost]
        [Route("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            string userKey = GetUserKey();
            Response ress = await _wishlistService.Delete(id, userKey);
            if (ress.Status == "success")
            {
                return Ok(ress);
            }
            return BadRequest(ress);
        }

        private string GetUserKey()
        {
            if (User.Identity.IsAuthenticated)
            {
                return User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
            }
            else
            {
                if (Request.Cookies["wishlistid"] == null)
                {
                    CookieOptions cookieOptions = new CookieOptions();
                    cookieOptions.Expires = DateTime.Now.AddMonths(2);
                    // Generate a new random GUID using System.Guid class.     
                    Guid tempCartId = Guid.NewGuid();
                    Response.Cookies.Append("wishlistid", tempCartId.ToString(), cookieOptions);
                    return tempCartId.ToString();
                }
                else
                {
                    CookieOptions cookieOptions = new CookieOptions();
                    cookieOptions.Expires = DateTime.Now.AddMonths(2);
                    Response.Cookies.Append("wishlistid", Request.Cookies["wishlistid"].ToString(), cookieOptions);
                    return Request.Cookies["wishlistid"].ToString();
                }
            }


        }
        
    }
}
