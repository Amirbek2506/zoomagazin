using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZooMag.Services.Interfaces;
using ZooMag.DTOs.AdditionalServ;

namespace ZooMag.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AdditionalServController : Controller
    {        
        private readonly IAdditionalServService _additionalServiсe;
         
        public AdditionalServController(IAdditionalServService service)
        {
            _additionalServiсe = service;
        }

        [HttpPost]
        [Authorize(Roles = "Администратор")]
        public async Task<IActionResult> CreateAdditionalServ([FromBody] CreateAdditionalServRequest request)
        {
            
        }

    }
}