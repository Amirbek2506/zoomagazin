using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using ZooMag.DTOs.Product;
using ZooMag.Entities;
using ZooMag.Models.ViewModels.Wishlist;
using ZooMag.Services.Interfaces;
using ZooMag.ViewModels;

namespace ZooMag.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class WishlistController : ControllerBase
    {
        private readonly IProductsService _productsService;
        private readonly UserManager<User> _userManager;

        public WishlistController(IProductsService productsService, UserManager<User> userManager)
        {
            _productsService = productsService;
            _userManager = userManager;
        }
        
        [HttpGet]
        public async Task<IActionResult> GetWishList()
        {
            string key = await GetUserKey();
            var response = await _productsService.GetWishListAsync(key);
            
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetWishlistCount()
        {
            string key = await GetUserKey();
            var response = await _productsService.GetWishlistCountAsync(key);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> AddToWishlist([FromBody]AddWishlistProductRequest productRequest)
        {
            string key = await GetUserKey();
            Response response = await _productsService.AddToWishlistAsync(key, productRequest.ProductItemId);
            return Created("Wishlist",response);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteFromWishlist([FromQuery] int productItemId)
        {
            string key = await GetUserKey();
            Response response = await _productsService.DeleteFromWishlistAsync(key,productItemId);
            
            return Created("Wishlist",response);
        }

        private async Task<string> GetUserKey()
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = await _userManager.GetUserAsync(User);
                return user.Id.ToString();
            }

            string key = HttpContext.Request.Cookies.ContainsKey("UserKey")
                ? HttpContext.Request.Cookies["UserKey"] : Guid.NewGuid().ToString();
            if (!HttpContext.Request.Cookies.ContainsKey("UserKey"))
            {
                HttpContext.Response.Cookies.Append("UserKey", key);
            }
            return key;
        }

        // [HttpPost]
        // [Route("create/{productid}")]
        // public async Task<IActionResult> Create(int productid)
        // {
        //     string userKey = GetUserKey();
        //     WishlistModel wishlistModel = 
        //         await _wishlistService.Create(
        //         new Wishlist 
        //         {
        //             ProductItemId = productid,
        //             UserId = userKey
        //         });
        //     if (wishlistModel != null)
        //     {
        //         return Ok(wishlistModel);
        //     }
        //     return BadRequest();
        // }
        //
        // [HttpGet]
        // [Route("fetch")]
        // public async Task<IActionResult> FetchItems()
        // {
        //     string userKey = GetUserKey();
        //     return Ok(await _wishlistService.FetchItems(userKey));
        // }
        //
        // [HttpGet]
        // [Route("count")]
        // public async Task<IActionResult> Count()
        // {
        //     string userKey = GetUserKey();
        //     return Ok(await _wishlistService.Count(userKey));
        // }
        //
        //
        // [HttpPost]
        // [Route("delete/{id}")]
        // public async Task<IActionResult> Delete(int id)
        // {
        //     string userKey = GetUserKey();
        //     Response ress = await _wishlistService.Delete(id, userKey);
        //     if (ress.Status == "success")
        //     {
        //         return Ok(ress);
        //     }
        //     return BadRequest(ress);
        // }
        //
        // private string GetUserKey()
        // {
        //     if (User.Identity.IsAuthenticated)
        //     {
        //         return User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
        //     }
        //     else
        //     {
        //         if (Request.Cookies["wishlistid"] == null)
        //         {
        //             CookieOptions cookieOptions = new CookieOptions();
        //             cookieOptions.Expires = DateTime.Now.AddMonths(2);
        //             // Generate a new random GUID using System.Guid class.     
        //             Guid tempCartId = Guid.NewGuid();
        //             Response.Cookies.Append("wishlistid", tempCartId.ToString(), cookieOptions);
        //             return tempCartId.ToString();
        //         }
        //         else
        //         {
        //             CookieOptions cookieOptions = new CookieOptions();
        //             cookieOptions.Expires = DateTime.Now.AddMonths(2);
        //             Response.Cookies.Append("wishlistid", Request.Cookies["wishlistid"].ToString(), cookieOptions);
        //             return Request.Cookies["wishlistid"].ToString();
        //         }
        //     }
        //
        //
        // }
        
    }
}
