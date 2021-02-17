using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ZooMag.Controllers
{
    public class CartController : Controller
    {
        public IActionResult Index()
        {
            return View();
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
                return Request.Cookies["cartid"].ToString();
            }
        }
    }
}
