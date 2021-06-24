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
        public async Task<IActionResult> Create([FromForm]CreateProductItemRequest request)
        {
            var response = await _productItemService.CreateAsync(request);
            return Created("ProductItem",response);
        }

        [HttpPut]
        [Authorize(Roles = "Администратор")]
        public async Task<IActionResult> Update([FromForm]UpdateProductItemRequest request)
        {
            var response = await _productItemService.UpdateAsync(request);
            return Created("ProductItem",response);
        }

        [HttpDelete]
        [Authorize(Roles = "Администратор")]
        public async Task<IActionResult> Delete([FromQuery]int id)
        {
            var response = await _productItemService.DeleteAsync(id);
            return Created("ProductItem",response);
        }
    }
}
