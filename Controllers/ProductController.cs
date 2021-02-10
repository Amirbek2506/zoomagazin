using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZooMag.Services.Interfaces;
using ZooMag.ViewModels;

namespace ZooMag.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductsService _productsService;

        public ProductController(IProductsService productsService)
        {
            this._productsService = productsService;
        }

        [HttpPost]
        [Route("create")]
        [Authorize(Roles = "Администратор")]
        public async Task<IActionResult> CreateProduct([FromForm] ProductModel model)
        {
            int productId = _productsService.CreateProduct(model);
            if (model.Images != null)
                await _productsService.CreateProductGaleries(productId, model.Images);
            if (model.InpSizes.Count() > 0)
            {
                List<int> Ids = await _productsService.CreateSizes(model.InpSizes);
                await _productsService.CreateProductSizes(productId, Ids);
            }
            return Ok(new Response { Status = "Success", Message = "Товар успешно добавлен!" });
        }


        [HttpGet]
        [Route("fetch")]
        public async Task<IActionResult> GetProducts()
        {
            var products = await _productsService.FetchProducts();
            foreach (var product in products)
            {
                product.ProductImages = await _productsService.FetchProductGaleriesByProductId(product.Id);
                product.OutSizes = await _productsService.FetchSizesByProductId(product.Id);
            }
            return Ok(products);
        }


        [HttpGet]
        [Route("fetchbyid")]
        public async Task<IActionResult> GetProductById(int id)
        {
            ProductModel productModel = _productsService.FetchProductById(id);
            if (productModel == null)
            {
                return StatusCode(
                    StatusCodes.Status400BadRequest,
                    new Response { Status = "Error", Message = "Товар не найден!" });
            }
            productModel.ProductImages = await _productsService.FetchProductGaleriesByProductId(id);
            productModel.OutSizes = await _productsService.FetchSizesByProductId(productModel.Id);

            return Ok(productModel);
        }


        [HttpPut]
        [Route("update")]
        [Authorize(Roles = "Администратор")]
        public async Task<IActionResult> UpdateProduct([FromForm] ProductModel model)
        {
            int productId = await _productsService.UpdateProduct(model);
            if(productId==0)
            {
                return StatusCode(
                    StatusCodes.Status400BadRequest,
                    new Response { Status = "Error", Message = "Товар не найден!" });
            }
            if (model.Images!=null)
                await _productsService.CreateProductGaleries(productId, model.Images);
            if (model.InpSizes.Count() > 0)
            {
                List<int> Ids = await _productsService.CreateSizes(model.InpSizes);
                await _productsService.CreateProductSizes(productId, Ids);
            }
            return Ok(new Response { Status = "Success", Message = "Товар успешно изменен!" });
        }

        [HttpDelete]
        [Route("delete")]
        [Authorize(Roles = "Администратор")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var res = await _productsService.DeleteProduct(id);
            if(res.Status == "success")
            {
                await _productsService.Save();
            }
            return Ok(res);
        }
    }
}
