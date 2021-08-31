using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZooMag.DTOs.FilterCategory;
using ZooMag.Services.Interfaces;
using ZooMag.ViewModels;

namespace ZooMag.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class FilterCategoryController : ControllerBase
    {
        private readonly IFilterCategoryService _filterCategoryService;

        public FilterCategoryController(IFilterCategoryService filterCategoryService)
        {
            _filterCategoryService = filterCategoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            List<FilterCategoryResponse> response = await _filterCategoryService.GetAllAsync();
            return Ok(response);
        }

        [HttpPost]
        [Authorize(Roles = "Администратор")]
        public async Task<IActionResult> Create(CreateFilterCategoryRequest request)
        {
            Response response = await _filterCategoryService.CreateAsync(request);
            return Created("FilterCategory", response);
        }
    }
}