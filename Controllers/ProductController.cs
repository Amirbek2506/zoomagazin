using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZooMag.Models.ViewModels.Products;
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
        public async Task<IActionResult> CreateProduct([FromForm] InpProductModel model)
        {
            int productId = await _productsService.CreateProduct(model);
            if (model.Images != null)
                await _productsService.CreateProductGaleries(productId, model.Images);
            if (model.Sizes.Count() > 0)
            {
                List<int> Ids = await _productsService.CreateSizes(model.Sizes);
                await _productsService.CreateProductSizes(productId, Ids);
            }
            return Ok(new Response { Status = "Success", Message = "Товар успешно добавлен!" });
        }



        [HttpGet]
        [Route("fetch")]
        public async Task<IActionResult> GetProducts(int offset=0, int limit=20,int categoryId=0)
        {
            var products = await _productsService.FetchProducts(limit<1?1:limit,offset<1?0:offset,categoryId);
            int count = await _productsService.CountProducts(categoryId);
            foreach (var product in products)
            {
                product.ProductImages = await _productsService.FetchProductGaleriesByProductId(product.Id);
                product.Sizes = await _productsService.FetchSizesByProductId(product.Id);
            }
            return Ok(new { count = count, products = products });
        }


        [HttpGet]
        [Route("fetchbyid/{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var productModel = _productsService.FetchProductById(id);
            if (productModel == null)
            {
                return BadRequest(new Response { Status = "Error", Message = "Товар не найден!" });
            }
            productModel.ProductImages = await _productsService.FetchProductGaleriesByProductId(id);
            productModel.Sizes = await _productsService.FetchSizesByProductId(productModel.Id);

            return Ok(productModel);
        }


        [HttpPut]
        [Route("update")]
        [Authorize(Roles = "Администратор")]
        public async Task<IActionResult> UpdateProduct([FromForm] UpdProductModel model)
        {
            int productId = await _productsService.UpdateProduct(model);
            if(productId==0)
            {
                return BadRequest(new Response { Status = "Error", Message = "Товар не найден!" });
            }
            if (model.Images!=null)
                await _productsService.CreateProductGaleries(productId, model.Images);
            if (model.Sizes.Count() > 0)
            {
                List<int> Ids = await _productsService.CreateSizes(model.Sizes);
                await _productsService.CreateProductSizes(productId, Ids);
            }
            return Ok(new Response { Status = "Success", Message = "Товар успешно изменен!" });
        }

        [HttpDelete]
        [Route("delete")]
        [Authorize(Roles = "Администратор")]
        public async Task<IActionResult> DeleteProduct([FromForm] int id)
        {
            var ress = await _productsService.DeleteProduct(id);
            if (ress.Status == "success")
            {
                return Ok(ress);
            }
            return BadRequest(ress);
        }

        [HttpDelete]
        [Route("deleteImage")]
        [Authorize(Roles = "Администратор")]
        public async Task<IActionResult> DeleteImage([FromForm]int imageId,[FromForm] int productId)
        {
            var ress = await _productsService.DeleteImage(imageId, productId);
            if (ress.Status == "success")
            {
                return Ok(ress);
            }
            return BadRequest(ress);
        }

        [HttpDelete]
        [Route("deleteSize")]
        [Authorize(Roles = "Администратор")]
        public async Task<IActionResult> DeleteSize([FromForm] int sizeId,[FromForm] int productId)
        {
            var ress = await _productsService.DeleteProductSize(productId,sizeId);
            if (ress.Status == "success")
            {
                return Ok(ress);
            }
            return BadRequest(ress);
        }
    }
}
