using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ZooMag.DTOs.Shop;
using ZooMag.Services.Interfaces;
using ZooMag.ViewModels;

namespace ZooMag.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ShopController : ControllerBase
    {
        private readonly IShopService _shopService;

        public ShopController(IShopService shopService)
        {
            _shopService = shopService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody]CreateShopRequest request)
        {
            Response response = await _shopService.CreateAsync(request);
            return Created("Shop",response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPickupPointsByCityId(int cityId)
        {
            var response = await _shopService.GetAllPickupPointsByCityIdAsync(cityId);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetById(int shopId)
        {
            var response = await _shopService.GetByIdAsync(shopId);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            List<ShopResponse> response = await _shopService.GetAllAsync();
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPickupPoints()
        {
            List<PickupPointResponse> response = await _shopService.GetAllPickupPointsAsync();
            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody]UpdateShopRequest request)
        {
            Response response = await _shopService.UpdateAsync(request);
            return Created("Shop",response);
        }
        
        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery]int id)
        {
            Response response = await _shopService.DeleteAsync(id);
            return Created("Shop",response);
        }
    }
}