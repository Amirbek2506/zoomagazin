using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZooMag.DTOs;
using ZooMag.DTOs.Product;
using ZooMag.DTOs.ProductItem;
using ZooMag.DTOs.Promotion;
using ZooMag.Entities;
using ZooMag.Services.Interfaces;
using ZooMag.ViewModels;

namespace ZooMag.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class PromotionController : ControllerBase
    {
        private readonly IPromotionService _promotionService;

        public PromotionController(IPromotionService promotionService)
        {
            _promotionService = promotionService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery]PagedRequest request)
        {
            var response = await _promotionService.GetAllAsync(request);
            return Ok(response);
        }

        [HttpPost]
        [Authorize(Roles = "Администратор")]
        public async Task<IActionResult> Create([FromForm]CreatePromotionRequest request)
        {
            var response = await _promotionService.CreateAsync(request);
            return Created("Promotion",response);
        }

        [HttpGet]
        public async Task<IActionResult> GetById([FromQuery]int id)
        {
            var response = await _promotionService.GetByIdAsync(id);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetPromotionProductItems([FromQuery]GenericPagedRequest<int> request)
        {
            var response = await _promotionService.GetPromotionProductItemsAsync(request);
            return Ok(response);
        }
    }
}