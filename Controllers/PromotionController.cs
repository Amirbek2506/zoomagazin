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
            GenericResponse<List<PromotionResponse>> response = await _promotionService.GetAllAsync(request);
            return Ok(response);
        }

        [HttpPost]
        [Authorize(Roles = "Администратор")]
        public async Task<IActionResult> Create([FromQuery]CreatePromotionRequest request)
        {
            Response response = await _promotionService.CreateAsync(request);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetById([FromQuery]int id)
        {
            PromotionResponse response = await _promotionService.GetByIdAsync(id);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetPromotionProductItems([FromQuery]GenericPagedRequest<int> request)
        {
            GenericResponse<List<ProductResponse>> response =
                await _promotionService.GetPromotionProductItemsAsync(request);
            return Ok(response);
        }
    }
}