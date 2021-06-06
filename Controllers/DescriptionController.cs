using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZooMag.DTOs.Description;
using ZooMag.Services.Interfaces;

namespace ZooMag.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class DescriptionController : ControllerBase
    {
        private readonly IDescriptionService _descriptionService;

        public DescriptionController(IDescriptionService descriptionService)
        {
            _descriptionService = descriptionService;
        }

        [HttpPost]
        [Authorize(Roles = "Администратор")]
        public async Task<IActionResult> Create([FromBody]CreateDescriptionRequest request)
        {
            var response = await _descriptionService.CreateAsync(request);
            return Ok(response);
        }

        [HttpPut]
        [Authorize(Roles = "Администратор")]
        public async Task<IActionResult> Update([FromBody]UpdateDescriptionRequest request)
        {
            var response = await _descriptionService.UpdateAsync(request);
            return Ok(response);
        }

        [HttpDelete]
        [Authorize(Roles = "Администратор")]
        public async Task<IActionResult> Delete([FromQuery] int id)
        {
            var response = await _descriptionService.DeleteAsync(id);
            return Ok(response);
        }
    }
}