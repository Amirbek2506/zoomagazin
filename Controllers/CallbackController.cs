using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZooMag.DTOs;
using ZooMag.DTOs.Callback;
using ZooMag.Services.Interfaces;

namespace ZooMag.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class CallbackController : ControllerBase
    {
        private readonly ICallbackService _callbackService;

        public CallbackController(ICallbackService callbackService)
        {
            _callbackService = callbackService;
        }

        [HttpPost]
        [Authorize(Roles = "Администратор")]
        public async Task<IActionResult> Create([FromBody]CreateCallbackRequest request)
        {
            var response = await _callbackService.CreateAsync(request);
            return Created("Callback",response);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll([FromQuery]PagedRequest request)
        {
            var response = await _callbackService.GetAllAsync(request);
            return Ok(response);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllNew()
        {
            var response = await _callbackService.GetAllNewAsync();
            return Ok(response);
        }
    }
}
