using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZooMag.DTOs;
using ZooMag.DTOs.City;
using ZooMag.Services.Interfaces;
using ZooMag.ViewModels;

namespace ZooMag.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class CityController : ControllerBase
    {
        private readonly ICityService _cityService;

        public CityController(ICityService cityService)
        {
            _cityService = cityService;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody]CreateCityRequest request)
        {
            var response = await _cityService.CreateAsync(request);
            return Created("City",response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync([FromQuery]PagedRequest request)
        {
            var response = await _cityService.GetAllAsync(request);
            return Ok(response);
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update([FromBody]UpdateCityRequest request)
        {
            var response = await _cityService.UpdateAsync(request);
            return Created("City", response);
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _cityService.DeleteAsync(id);
            return Created("City", response);
        }
    }
}