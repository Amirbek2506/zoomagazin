using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
            if (model.Sizes!=null)
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
                product.Images = await _productsService.FetchProductGaleriesByProductId(product.Id);
                product.Sizes = await _productsService.FetchSizesByProductId(product.Id);
            }
            return Ok(new { count = count, products = products });
        }
        

        [HttpGet]
        [Route("fetchsales/{count}")]
        public async Task<IActionResult> GetSales(int count=10)
        {
            var products = await _productsService.FetchSales(count);
            foreach (var product in products)
            {
                product.Images = await _productsService.FetchProductGaleriesByProductId(product.Id);
                product.Sizes = await _productsService.FetchSizesByProductId(product.Id);
            }
            return Ok(products);
        }

        [HttpGet]
        [Route("fetchnew/{count}")]
        public async Task<IActionResult> GetNew(int count=10)
        {
            var products = await _productsService.FetchNew(count);
            foreach (var product in products)
            {
                product.Images = await _productsService.FetchProductGaleriesByProductId(product.Id);
                product.Sizes = await _productsService.FetchSizesByProductId(product.Id);
            }
            return Ok(products);
        }

        [HttpGet]
        [Route("search")]
        public async Task<IActionResult> Search(string q,int offset=0, int limit=20,int categoryId=0)
        {
            var products = await _productsService.Search(limit<1?1:limit,offset<1?0:offset,categoryId,q);
            int count = await _productsService.SearchCount(categoryId,q);
            foreach (var product in products)
            {
                product.Images = await _productsService.FetchProductGaleriesByProductId(product.Id);
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
            productModel.Images = await _productsService.FetchProductGaleriesByProductId(id);
            productModel.Sizes = await _productsService.FetchSizesByProductId(productModel.Id);

            return Ok(productModel);
        }

        [HttpPost]
        [Route("fetchbyids")]
        public async Task<IActionResult> GetProductByIds([FromForm] int[] ids)
        {
            var productModels = await _productsService.FetchProductByIds(ids);
            if (productModels == null)
            {
                return BadRequest(new Response { Status = "Error", Message = "Товар не найден!" });
            }
            foreach(var productModel in productModels)
            {
                productModel.Images = await _productsService.FetchProductGaleriesByProductId(productModel.Id);
                productModel.Sizes = await _productsService.FetchSizesByProductId(productModel.Id);
            }
            return Ok(productModels);
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
           
            if (model.Sizes!=null)
            {
                List<int> Ids = await _productsService.CreateSizes(model.Sizes);
                await _productsService.CreateProductSizes(productId, Ids);
            }
            return Ok(new Response { Status = "Success", Message = "Товар успешно изменен!" });
        }

        [HttpPost]
        [Route("createimages")]
        [Authorize(Roles = "Администратор")]
        public async Task<IActionResult> CreateImages([FromForm]int productid, IFormFile[] Images)
        {
            var productModel = _productsService.FetchProductById(productid);
            if (productModel == null)
            {
                return BadRequest(new Response { Status = "Error", Message = "Товар не найден!" });
            }
            if (Images!=null)
            {
                await _productsService.CreateProductGaleries(productid, Images);
            }
            productModel.Images = await _productsService.FetchProductGaleriesByProductId(productid);

            return Ok(productModel);
        }

        [HttpPost]
        [Route("setmainimage")]
        [Authorize(Roles = "Администратор")]
        public async Task<IActionResult> SetMainImage(int productid, int imageid)
        {
            var ress = await _productsService.SetMainImage(productid,imageid);
            if(ress.Status == "success")
            {
                return Ok(ress);
            }
            return BadRequest(ress);
        }

        [HttpDelete]
        [Route("delete/{id}")]
        [Authorize(Roles = "Администратор")]
        public async Task<IActionResult> DeleteProduct(int id)
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


        [HttpGet]
        [Route("fetchsizes/{productid}")]
        public async Task<IActionResult> FetchSizes(int productid)
        {
            return Ok(await _productsService.FetchSizesByProductId(productid));
        }

    }
}
