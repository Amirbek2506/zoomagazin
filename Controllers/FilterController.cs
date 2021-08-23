using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZooMag.DTOs.Filter;
using ZooMag.Services.Interfaces;

namespace ZooMag.Controllers
{
    public class FilterController : ControllerBase
    {
        private readonly IFilterService _filterService;

        public FilterController(IFilterService filterService)
        {
            _filterService = filterService;
        }
        
        [HttpPost]
        [Authorize(Roles = "Администратор")]
        public async Task<IActionResult> Create(CreateFilterRequest request)
        {
            var response = await _filterService.CreateAsync(request);
            return Created("Filter", response);
        }

        [HttpGet]
        public async Task<IActionResult> GetFilterForSelectOption()
        {
            var response = await _filterService.GetFilterForSelectOptionAsync();
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await _filterService.GetAllAsync();
            return Ok(response);
        }
    }
}