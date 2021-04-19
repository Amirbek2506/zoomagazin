using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZooMag.Services;
using ZooMag.Services.Interfaces;
using ZooMag.ViewModels;

namespace ZooMag.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _usersService;

        public UserController(IUserService usersService)
        {
            this._usersService = usersService;
        }

        [HttpGet]
        [Route("fetchWorkers")]
        [Authorize(Roles = "Администратор")]
        public async Task<IActionResult> GetFetchWorkers(int offset = 0, int limit = 20)
        {
            return Ok(new { count = await _usersService.CountWorkers(), users = await _usersService.FetchWorkers(offset, limit) });
        }

        [HttpGet]
        [Route("fetchClients")]
        [Authorize(Roles = "Администратор")]
        public async Task<IActionResult> FetchСlients(int offset = 0, int limit = 20)
        {
            return Ok(new { count = await _usersService.CountClients(), users = await _usersService.FetchСlients(offset, limit) });
        }

        
        [HttpGet]
        [Route("fetchRoles")]
        [Authorize(Roles = "Администратор")]
        public async Task<IActionResult> GetRoles()
        {
            return Ok(await _usersService.GetRoles());
        }
        
        [HttpGet]
        [Route("fetchGenders")]
        public async Task<IActionResult> GetGenders()
        {
            return Ok(await _usersService.GetGenders());
        }
        
        [HttpPost]
        [Route("setRole")]
        [Authorize(Roles = "Администратор")]
        public async Task<IActionResult> SetRole([FromForm]int userId,[FromForm]int roleId)
        {
           Response ress = await _usersService.SetRole(userId, roleId);
            if(ress.Status == "success")
            {
                return Ok(ress);
            }
            return BadRequest(ress);
        }

        [HttpPut]
        [Route("update")]
        public async Task<IActionResult> Update([FromForm]UserModel userModel)
        {
            return Ok(await _usersService.UpdateUser(userModel));
        }


        [HttpDelete]
        [Route("delete")]
        [Authorize(Roles = "Администратор")]
        public async Task<IActionResult> DeleteUser([FromForm] int id)
        {
           Response ress = await _usersService.DeleteUser(id);
            if(ress.Status == "success")
            {
                return Ok(ress);
            }
            return BadRequest(ress);
        }

    }
}
