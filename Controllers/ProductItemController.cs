using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZooMag.Data;
using ZooMag.DTOs.ProductItem;
using ZooMag.Services.Interfaces;
using ZooMag.ViewModels;

namespace ZooMag.Controllers
{
    [Route("/[controller]/[action]")]
    [ApiController]
    public class ProductItemController : ControllerBase
    {
        private readonly IProductItemService _productItemService;

        public ProductItemController(IProductItemService productItemService)
        {
            _productItemService = productItemService;
        }

        [HttpPost]
        [Authorize(Roles = "Администратор")]
        public async Task<IActionResult> Create([FromBody]CreateProductItemRequest request)
        {
            var response = await _productItemService.CreateAsync(request);
            return Ok(response);
        }

        [HttpPut]
        [Authorize(Roles = "Администратор")]
        public async Task<IActionResult> Update([FromBody]UpdateProductItemRequest request)
        {
            var response = await _productItemService.UpdateAsync(request);
            return Ok(response);
        }

        [HttpDelete]
        [Authorize(Roles = "Администратор")]
        public async Task<IActionResult> Delete([FromQuery]int id)
        {
            var response = await _productItemService.DeleteAsync(id);
            return Ok(response);
        }
    }
}
