using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZooMag.DTOs.SpecificFilter;
using ZooMag.Services.Interfaces;
using ZooMag.ViewModels;

namespace ZooMag.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class SpecificFilterController : ControllerBase
    {
        private readonly ISpecificFilterService _specificFilterService;

        public SpecificFilterController(ISpecificFilterService specificFilterService)
        {
            _specificFilterService = specificFilterService;
        }

        [HttpPost]
        [Authorize(Roles = "Администратор")]
        public async Task<IActionResult> Create(CreateSpecificFilterRequest request)
        {
            Response response = await _specificFilterService.CreateAsync(request);
            return Created("SpecificFilter",response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            List<SpecificFilterResponse> response = await _specificFilterService.GetAllAsync();
            return Ok(response);
        }
    }
}