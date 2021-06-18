using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using ZooMag.DTOs;
using ZooMag.DTOs.Product;
using ZooMag.DTOs.ProductItem;
using ZooMag.Entities;
using ZooMag.Helpers;
using ZooMag.Services.Interfaces;
using ZooMag.ViewModels;

namespace ZooMag.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductsService _productsService;
        private readonly UserManager<User> _userManager;

        public ProductController(IProductsService productsService, UserManager<User> userManager)
        {
            this._productsService = productsService;
            _userManager = userManager;
        }

        [HttpPost]
        [Authorize(Roles = "Администратор")]
        public async Task<IActionResult> Create( CreateProductRequest request)
        {
            var response = await _productsService.CreateAsync(request);
            return Ok(response);
        }

        [HttpPut]
        [Authorize(Roles = "Администратор")]
        public async Task<IActionResult> Update([FromBody]UpdateProductRequest request)
        {
            var response = await _productsService.UpdateAsync(request);
            return Ok(response);
        }


        [HttpDelete]
        [Authorize(Roles = "Администратор")]
        public async Task<IActionResult> Delete([FromQuery] int id)
        {
            var response = await _productsService.DeleteAsync(id);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetProductsByBrandId([FromQuery]GenericPagedRequest<int> request)
        {
            var response = await _productsService.GetProductsByBrandIdAsync(request);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetMostPopularByCategoryId([FromQuery]GenericPagedRequest<int> request)
        {
            var response = await _productsService.GetMostPopularAsync(request);
            return Ok(response);
        }
        
        [HttpGet]
        public async Task<IActionResult> Search([FromQuery]GenericPagedRequest<string> request)
        {
            var response = await _productsService.SearchAsync(request);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery]PagedRequest request)
        {
            var response = await _productsService.GetAllAsync(request);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetWishList()
        {
            string key = await GetUserKey();
            List<WishListProductItemResponse> response = await _productsService.GetWishListAsync(key);
            
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetWishlistCount()
        {
            string key = await GetUserKey();
            var response = await _productsService.GetWishlistCountAsync(key);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> AddToWishlist([FromBody]AddWishlistProductRequest productRequest)
        {
            string key = await GetUserKey();
            Response response = await _productsService.AddToWishlistAsync(key, productRequest.ProductItemId);
            return Ok(response);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteFromWishlist([FromQuery] int productItemId)
        {
            string key = await GetUserKey();
            Response response = await _productsService.DeleteFromWishlistAsync(key,productItemId);
            
            return Ok(response);
        }

        private async Task<string> GetUserKey()
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = await _userManager.GetUserAsync(User);
                return user.Id.ToString();
            }

            string key = HttpContext.Request.Cookies.ContainsKey("UserKey")
                ? HttpContext.Request.Cookies["UserKey"] : Guid.NewGuid().ToString();
            KeyValuePair<string, string> userKey = new KeyValuePair<string, string>("","");
            if (!HttpContext.Request.Cookies.ContainsKey("UserKey"))
            {
                HttpContext.Request.Cookies.Append(new KeyValuePair<string, string>("UserKey", key));
            }
            return key;
        }

        [HttpPost]
        public async Task<IActionResult> AddToBasket([FromBody]AddToBusketRequest request)
        {
            string key = await GetUserKey();
            Response response = await _productsService.AddToBasketAsync(key,request);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetBasketProducts()
        {
            string key = await GetUserKey();
            List<BasketProductResponse> response = await _productsService.GetBasketProductsAsync(key);
            
            return Ok(response);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteBasketProduct([FromQuery] int productItemId)
        {
            string key = await GetUserKey();
            Response response = await _productsService.DeleteBasketProductAsync(productItemId,key);
            return Ok(response);
        }

        [HttpDelete]
        public async Task<IActionResult> DecreaseBasketProduct([FromQuery] int productItemId)
        {
            string key = await GetUserKey();
            Response response = await _productsService.DecreaseBasketProductAsync(productItemId, key);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetFilteredProducts([FromQuery]GenericPagedRequest<ProductFiltersRequest> request)
        {
            var response = await _productsService.GetFilteredProductsAsync(request);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetProductDetails([FromQuery] int id)
        {
            ProductDetailsResponse response = await _productsService.GetProductDetailsAsync(id);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetProductItemDetails([FromQuery] int productItemId)
        {
            ProductItemDetailsResponse response = await _productsService.GetProductItemDetailsAsync
                (productItemId);
            return Ok(response);
        }
        //[HttpPost]
        //[Route("create")]
        //[Authorize(Roles = "Администратор")]
        //public async Task<IActionResult> CreateProduct([FromBody] InpProductModel model)
        //{
        //    int productId = await _productsService.CreateProduct(model);

        //    return Ok(new Response { Status = "Success", Message = "Товар успешно добавлен!" });
        //}



        //[HttpGet]
        //[Route("fetch")]
        //public async Task<IActionResult> GetProducts([FromQuery]FetchProductsRequest request)//int offset=0, int limit=20,int categoryId=0,int brandId=0, int minp=0,int maxp=0,bool issale=false,bool isnew=false,bool istop=false,bool isrecommended=false)
        //{
        //    var products = await _productsService.FetchProducts(request); //limit<1?1:limit,offset<1?0:offset,categoryId, brandId,minp, maxp,issale,isnew,istop,isrecommended);
        //    // int count = await _productsService.CountProducts(categoryId,brandId,minp,maxp,issale,isnew,istop,isrecommended);
        //    // foreach (var product in products)
        //    // {
        //    //     product.Images = await _productsService.FetchProductGaleriesByProductId(product.Id);
        //    // }
        //    return Ok(new { count = products.Count, products = products });
        //}


        //[HttpGet]
        //[Route("fetchsales/{count}")]
        //public async Task<IActionResult> GetSales(int count=10)
        //{
        //    var products = await _productsService.FetchSales(count);
        //    // foreach (var product in products)
        //    // {
        //    //     product.Images = await _productsService.FetchProductGaleriesByProductId(product.Id);
        //    // }
        //    return Ok(products);
        //}

        //[HttpGet]
        //[Route("fetchtopes/{count}")]
        //public async Task<IActionResult> GetTopes(int count = 10)
        //{
        //    var products = await _productsService.FetchTopes(count);
        //    // foreach (var product in products)
        //    // {
        //    //     product.Images = await _productsService.FetchProductGaleriesByProductId(product.Id);
        //    // }
        //    return Ok(products);
        //}

        //[HttpGet]
        //[Route("fetchrecommended/{count}")]
        //public async Task<IActionResult> GetRecommended(int count = 10)
        //{
        //    var products = await _productsService.FetchRecommended(count);
        //    // foreach (var product in products)
        //    // {
        //    //     product.Images = await _productsService.FetchProductGaleriesByProductId(product.Id);
        //    // }
        //    return Ok(products);
        //}


        //[HttpGet]
        //[Route("fetchnew/{count}")]
        //public async Task<IActionResult> GetNew(int count=10)
        //{
        //    var products = await _productsService.FetchNew(count);
        //    // foreach (var product in products)
        //    // {
        //    //     product.Images = await _productsService.FetchProductGaleriesByProductId(product.Id);
        //    // }
        //    return Ok(products);
        //}


        //[HttpGet]
        //[Route("search")]
        //public async Task<IActionResult> Search(string q,int offset=0, int limit=20,int categoryId=0)
        //{
        //    var products = await _productsService.Search(limit<1?1:limit,offset<1?0:offset,categoryId,q);
        //    int count = await _productsService.SearchCount(categoryId,q);
        //    return Ok(new { count = count, products = products });
        //}


        //[HttpGet]
        //[Route("fetchbyid/{id}")]
        //public async Task<IActionResult> GetProductById(int id)
        //{
        //    var productModel = _productsService.FetchProductById(id);
        //    if (productModel == null)
        //    {
        //        return BadRequest(new Response { Status = "Error", Message = "Товар не найден!" });
        //    }
        //    productModel.Images = await _productsService.FetchProductGaleriesByProductId(id);

        //    return Ok(productModel);
        //}


        //[HttpPost]
        //[Route("fetchbyids")]
        //public async Task<IActionResult> GetProductByIds([FromForm] int[] ids)
        //{
        //    var productModels = await _productsService.FetchProductByIds(ids);
        //    if (productModels == null)
        //    {
        //        return BadRequest(new Response { Status = "Error", Message = "Товар не найден!" });
        //    }
        //    // foreach(var productModel in productModels)
        //    // {
        //    //     productModel.Images = await _productsService.FetchProductGaleriesByProductId(productModel.Id);
        //    // }
        //    return Ok(productModels);
        //}


        //[HttpPut]
        //[Route("update")]
        //[Authorize(Roles = "Администратор")]
        //public async Task<IActionResult> UpdateProduct([FromForm] UpdProductModel model)
        //{
        //    int productId = await _productsService.UpdateProduct(model);
        //    if(productId==0)
        //    {
        //        return BadRequest(new Response { Status = "Error", Message = "Товар не найден!" });
        //    }

        //    return Ok(new Response { Status = "Success", Message = "Товар успешно изменен!" });
        //}

        //[HttpPost]
        //[Route("createimages")]
        //[Authorize(Roles = "Администратор")]
        //public async Task<IActionResult> CreateImages([FromForm]int productid, IFormFile[] Images)
        //{
        //    var productModel = _productsService.FetchProductById(productid);
        //    if (productModel == null)
        //    {
        //        return BadRequest(new Response { Status = "Error", Message = "Товар не найден!" });
        //    }
        //    if (Images!=null)
        //    {
        //        await _productsService.CreateProductGaleries(productid, Images);
        //    }
        //    productModel.Images = await _productsService.FetchProductGaleriesByProductId(productid);

        //    return Ok(productModel);
        //}

        //[HttpPost]
        //[Route("setmainimage")]
        //[Authorize(Roles = "Администратор")]
        //public async Task<IActionResult> SetMainImage(int productid, int imageid)
        //{
        //    var ress = await _productsService.SetMainImage(productid,imageid);
        //    if(ress.Status == "success")
        //    {
        //        return Ok(ress);
        //    }
        //    return BadRequest(ress);
        //}



        //[HttpDelete]
        //[Route("delete/{id}")]
        //[Authorize(Roles = "Администратор")]
        //public async Task<IActionResult> DeleteProduct(int id)
        //{
        //    var ress = await _productsService.DeleteProduct(id);
        //    if (ress.Status == "success")
        //    {
        //        return Ok(ress);
        //    }
        //    return BadRequest(ress);
        //}



        //[HttpDelete]
        //[Route("deleteImage")]
        //[Authorize(Roles = "Администратор")]
        //public async Task<IActionResult> DeleteImage([FromForm]int imageId,[FromForm] int productId)
        //{
        //    var ress = await _productsService.DeleteImage(imageId, productId);
        //    if (ress.Status == "success")
        //    {
        //        return Ok(ress);
        //    }
        //    return BadRequest(ress);
        //}
    }
}
