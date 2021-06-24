using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ZooMag.DTOs.PickupPoint;
using ZooMag.Services.Interfaces;
using ZooMag.ViewModels;

namespace ZooMag.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class PickupPointController : ControllerBase
    {
        private readonly IPickupPointService _pickupPointService;

        public PickupPointController(IPickupPointService pickupPointService)
        {
            _pickupPointService = pickupPointService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody]CreatePickupPointRequest request)
        {
            Response response = await _pickupPointService.CreateAsync(request);
            return Created("PickupPoint",response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            List<PickupPointResponse> response = await _pickupPointService.GetAllAsync();
            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody]UpdatePickupPointRequest request)
        {
            Response response = await _pickupPointService.UpdateAsync(request);
            return Created("PickupPoint",response);
        }
        
        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery]int id)
        {
            Response response = await _pickupPointService.DeleteAsync(id);
            return Created("PickupPoint",response);
        }
    }
}