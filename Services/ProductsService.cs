using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using ZooMag.Data;
using ZooMag.DTOs.Description;
using ZooMag.DTOs.Product;
using ZooMag.DTOs.ProductItem;
using ZooMag.Entities;
using ZooMag.Mapping;
//using ZooMag.Models;
using ZooMag.Services.Interfaces;
using ZooMag.ViewModels;

namespace ZooMag.Services
{
    public class ProductsService : IProductsService
    {
        private readonly ApplicationDbContext _context;
        private readonly IFileService _fileService;
        private readonly IProductItemService _productItemService;
        private readonly IMapper _mapper;

        public ProductsService(ApplicationDbContext context, IFileService fileService, IProductItemService productItemService)
        {
            _context = context;
            _fileService = fileService;
            _productItemService = productItemService;
            _mapper = new MapperConfiguration(x => x.AddProfile<GeneralProfile>()).CreateMapper();
        }

        public async Task<Response> CreateAsync(CreateProductRequest request)
        {
            if (request.ProductItems.Count == 0)
                return new Response { Message = "", Status = "error" };
            Dictionary<string, List<string>> productItemImages = new Dictionary<string, List<string>>();

            string emptyFilePath = Path.GetFullPath("Resources/Images/Products/image.png");
            
            foreach(var productItem in request.ProductItems)
            {
                var productImages = await _fileService.AddProductItemFilesASync(productItem.Images);
                productItemImages.Add(productItem.VendorCode, productImages.Count > 0 ? productImages : new List<string> { emptyFilePath });
            }

            Dictionary<string, List<Description>> productItemDescriptions = request.ProductItems.ToDictionary(
                x => x.VendorCode, x => x.Descriptions.Select(x => new Description
                {
                    Content = x.Content,
                    Title = x.Title
                }).ToList());

            var product = new Product
            {
                BrandId = request.BrandId,
                CategoryId = request.CategoryId,
                CreateDate = DateTime.Now,
                Title = request.Title,
                TitleDescription = request.TitleDescription,
                Removed = false,
                ProductItems = request.ProductItems.Select(x => new ProductItem
                {
                    Measure = x.Measure,
                    Percent = x.Percent,
                    Price = x.Price,
                    VendorCode = x.VendorCode,
                    ProductItemImages = productItemImages[x.VendorCode].Select(pi => new ProductItemImage
                    {
                        ImagePath = pi
                    }).ToList(),
                    Descriptions = productItemDescriptions[x.VendorCode]
                }).ToList()
            };

            await _context.Products.AddAsync(product);

            await _context.SaveChangesAsync();

            return new Response { Status = "success", Message = "" };
        }

        public async Task<Response> UpdateAsync(UpdateProductRequest request)
        {
            var product = await _context.Products.FindAsync(request.Id);
            if (product == null)
                return new Response { Status = "error", Message = "Продукт не найден" };

            product.BrandId = request.BrandId;
            product.CategoryId = request.CategoryId;
            product.Title = request.Title;
            product.TitleDescription = request.TitleDescription;

            await _context.SaveChangesAsync();

            return new Response { Status = "success", Message = "Успешно изменен" };
        }

        public async Task<Response> DeleteAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return new Response {Status = "error", Message = "Не найден"};
            product.Removed = true;
            var productItems = await _context.ProductItems.Where(x => x.ProductId == id).ToListAsync();

            var productItemIds = productItems.Select(x => x.Id).ToList();
            
            foreach (var productItem in productItems)
            {
                productItem.Removed = true;
            }

            string deletedFilePath = Path.GetFullPath("Resources/Images/deleted.png");

            var newProductItemImages =
                productItemIds.Select(x => new ProductItemImage {ImagePath = deletedFilePath, ProductItemId = x}).ToList();

            var productItemImages = await _context.ProductItemImages
                .Where(x => productItemIds.Contains(x.ProductItemId)).ToListAsync();

            foreach (var productItemImage in productItemImages)
            {
                _fileService.Delete(productItemImage.ImagePath);
            }
            
            _context.ProductItemImages.RemoveRange(productItemImages);

            await _context.ProductItemImages.AddRangeAsync(newProductItemImages);
            
            await _context.SaveChangesAsync();

            return new Response {Message = "Успешно", Status = "success"};
        }

        public async Task<List<MostPopularProductResponse>> GetMostPopularAsync(int categoryId)
        {
            var categories = await _context.Categories.ToListAsync();
            List<int> categoryIds = new List<int> {categoryId};
            GetParentCategoryCategories(ref categoryIds,categoryId,categories);

            return await _context.Products.Where(x => categoryIds.Contains(x.CategoryId) && !x.Removed)
                .Include(x => x.ProductItems).ThenInclude(x=>x.ProductItemImages)
                .Select(x => new MostPopularProductResponse
                {
                    Id = x.Id,
                    Title = x.Title,
                    TitleDescription = x.TitleDescription,
                    ImagePath = x.ProductItems.First(pi=>!pi.Removed).ProductItemImages.First().ImagePath,
                    ProductItems = x.ProductItems.Where(pi=>!pi.Removed).Select(pi => new MostPopularProductItemResponse
                    {
                        Discount = pi.Percent,
                        Measure = pi.Measure,
                        Price = pi.Price,
                        SellingPrice = Math.Round(pi.Price - pi.Price * pi.Percent / 100,2),
                        Id = pi.Id
                    }).ToList()
                }).ToListAsync();
        }

        public async Task<List<SearchProductResponse>> SearchAsync(string query)
        {
            var categories = await _context.Categories.Where(x => x.Name.Contains(query)).Select(x => x.Id)
                .ToListAsync();

            return await _context.Products.Where(x=>!x.Removed).Include(x => x.ProductItems).ThenInclude(x => x.ProductItemImages)
                .Where(x => categories.Contains(x.CategoryId) || x.Title.Contains(query) ||
                                           x.ProductItems.Any(pi => pi.VendorCode.Contains(query)))
                .Select(x => new SearchProductResponse
                {
                    Id = x.Id,
                    Price = x.ProductItems.First(pi=>!pi.Removed).Price,
                    Title = x.Title,
                    ImagePath = x.ProductItems.First(pi=>!pi.Removed).ProductItemImages.First().ImagePath
                }).ToListAsync();
        }

        public async Task<List<ProductResponse>> GetAllAsync()
        {
            return await _context.Products.Where(x=>!x.Removed).Include(x=>x.ProductItems).ThenInclude(x=>x.ProductItemImages)
                .Select(x => new ProductResponse
                {
                    Id = x.Id,
                    Title = x.Title,
                    TitleDescription = x.TitleDescription,
                    ImagePath = x.ProductItems.First(pi=>!pi.Removed).ProductItemImages.First().ImagePath,
                    ProductItems = x.ProductItems.Where(pi=>!pi.Removed).Select(pi => new ProductItemResponse
                    {
                        Discount = pi.Percent,
                        Measure = pi.Measure,
                        Price = pi.Price,
                        SellingPrice = Math.Round(pi.Price - pi.Price * pi.Percent / 100,2),
                        Id = pi.Id
                    }).ToList()
                }).ToListAsync();
        }

        public async Task<List<WishListProductItemResponse>> GetWishListAsync(string key)
        {
            return await _context.Wishlists.Where(x => x.UserId == key).Include(x => x.ProductItem)
                .ThenInclude(x => x.Product)
                .Include(x => x.ProductItem).ThenInclude(x => x.ProductItemImages)
                .Select(x => new WishListProductItemResponse
                {
                    Id = x.ProductItemId,
                    Price = x.ProductItem.Price,
                    Title = x.ProductItem.Product.Title,
                    ImagePath = x.ProductItem.ProductItemImages.First().ImagePath
                }).ToListAsync();
        }

        public async Task<int> GetWishlistCountAsync(string key)
        {
            return await _context.Wishlists.Where(x => x.UserId == key).CountAsync();
        }

        public async Task<Response> AddToWishlistAsync(string key, int productItemId)
        {

            var wishlist =
                await _context.Wishlists.FirstOrDefaultAsync(x => x.UserId == key && x.ProductItemId == productItemId);
            if (wishlist != null)
                return new Response {Status = "error", Message = "Продукт уже добавлен"};
            wishlist = new WishList
            {
                UserId = key,
                ProductItemId = productItemId
            };
            await _context.Wishlists.AddAsync(wishlist);
            await _context.SaveChangesAsync();
            return new Response {Message = "Успешно", Status = "success"};
        }

        public async Task<Response> DeleteFromWishlistAsync(string key, int productItemId)
        {
            var wishlist =
                await _context.Wishlists.FirstOrDefaultAsync(x => x.UserId == key && x.ProductItemId == productItemId);
            if (wishlist == null)
                return new Response {Status = "error", Message = "Не найден"};
            _context.Wishlists.Remove(wishlist);
            await _context.SaveChangesAsync();
            return new Response {Message = "Успешно", Status = "success"};
        }

        public async Task<Response> AddToBasketAsync(string key,AddToBusketRequest request)
        {
            var basket =
                await _context.Baskets.FirstOrDefaultAsync(x =>
                    x.UserId == key && x.ProductItemId == request.ProductItemId);
            if (basket != null)
                basket.Count += request.Count;
            else
            {
                basket = new Basket
                {
                    Count = request.Count,
                    ProductItemId = request.ProductItemId,
                    UserId = key
                };
                await _context.Baskets.AddAsync(basket);
            }

            await _context.SaveChangesAsync();
            return new Response {Message = "Успешно", Status = "success"};
        }

        public async Task<List<BasketProductResponse>> GetBasketProductsAsync(string key)
        {
            return await _context.Baskets.Where(x => x.UserId == key).Include(x => x.ProductItem)
                .ThenInclude(x => x.Product).Include(x => x.ProductItem).ThenInclude(x => x.ProductItemImages)
                .Select(x => new BasketProductResponse
                {
                    Count = x.Count,
                    Measure = x.ProductItem.Measure,
                    Percent = x.ProductItem.Percent,
                    Title = x.ProductItem.Product.Title,
                    ImagePath = x.ProductItem.ProductItemImages.First().ImagePath,
                    SellingPrice = x.ProductItem.Price - Math.Round(x.ProductItem.Price * x.ProductItem.Percent / 100,2),
                    ProductId = x.ProductItem.ProductId,
                    Price = x.ProductItem.Price,
                    TitleDescription = x.ProductItem.Product.TitleDescription,
                    VendorCode = x.ProductItem.VendorCode,
                    ProductItemId = x.ProductItemId,
                    Benefit = Math.Round(x.ProductItem.Price * x.ProductItem.Percent / 100,2) * x.Count,
                    Summa = Math.Round(x.ProductItem.Price - x.ProductItem.Price * x.ProductItem.Percent / 100,2) * x.Count
                }).ToListAsync();
        }

        public async Task ChangeWishlistProductsUserIdAsync(string key,string newKey)
        {
            var wishLists = await _context.Wishlists.Where(x => x.UserId == key).ToListAsync();
            var wishListsId = await _context.Wishlists.Where(x => x.UserId == key).Select(x=> x.ProductItemId).ToListAsync();
            var oldWishlists = await _context.Wishlists.Where(x => x.UserId == newKey).Select(x=> x.ProductItemId).ToListAsync();
            
            var newWishlists = wishListsId.Except(oldWishlists).Select(x => new WishList {UserId = newKey, ProductItemId = x}).ToList();
            _context.Wishlists.RemoveRange(wishLists);
            await _context.Wishlists.AddRangeAsync(newWishlists);
            await _context.SaveChangesAsync();
        }

        public async Task ChangeBasketProductsUserIdAsync(string key, string newKey)
        {
            var basketProducts = await _context.Baskets.Where(x => x.UserId == key).ToListAsync();
            var oldBasketProducts = await _context.Baskets.Where(x => x.UserId == newKey).ToListAsync();
            var oldBasketProductsId = oldBasketProducts.Select(x => x.ProductItemId).ToList();
            var newBasketProducts = new List<Basket>();
            for (int i = 0; i < basketProducts.Count; i++)
            {
                if (oldBasketProductsId.Contains(basketProducts[i].ProductItemId))
                {
                    var oldBasketProduct = oldBasketProducts.First(x =>
                        x.UserId == newKey && x.ProductItemId == basketProducts[i].ProductItemId);
                    oldBasketProduct.Count += basketProducts[i].Count;
                    _context.Baskets.Update(oldBasketProduct);
                }
                else
                {
                    var newBasketProduct = basketProducts
                        .Select(x => new Basket {Count = x.Count, UserId = newKey, ProductItemId = x.ProductItemId})
                        .FirstOrDefault(x => x.ProductItemId == basketProducts[i].ProductItemId);
                    newBasketProducts.Add(newBasketProduct);
                }
            }
            _context.Baskets.RemoveRange(basketProducts);
            await _context.Baskets.AddRangeAsync(newBasketProducts);
            await _context.SaveChangesAsync();
        }

        public async Task<Response> DeleteBasketProductAsync(int productItemId,string key)
        {
            var basketProduct =
                await _context.Baskets.FirstOrDefaultAsync(x => x.UserId == key && x.ProductItemId == productItemId);
            _context.Baskets.Remove(basketProduct);
            await _context.SaveChangesAsync();
            return new Response {Status = "success", Message = "Успешно"};
        }

        public async Task<Response> DecreaseBasketProductAsync(int productItemId, string key)
        {
            var basketProduct =
                await _context.Baskets.FirstOrDefaultAsync(x => x.UserId == key && x.ProductItemId == productItemId);
            if (basketProduct == null)
                return new Response {Message = "Не найден", Status = "error"};
            basketProduct.Count--;
            _context.Update(basketProduct);
            await _context.SaveChangesAsync();
            return new Response {Status = "success", Message = "Успешно"};
        }

        public async Task<List<ProductResponse>> GetFilteredProductsAsync(ProductFiltersRequest request)
        {
            if (request.MaxPrice < request.MinPrice)
                return new List<ProductResponse>();
            var categories = await _context.Categories.ToListAsync();
            List<int> categoryIds = request.CategoriesId;
            for (int i = 0; i < request.CategoriesId?.Count; i++)
            {
                GetParentCategoryCategories(ref categoryIds,request.CategoriesId[i],categories);
            }
            return await _context.Products
                .Where(x => !x.Removed && (request.CategoriesId == null || categoryIds.Contains(x.CategoryId)) && (request.BrandsId == null || request.BrandsId.Contains(x.BrandId)))
                .Include(x => x.ProductItems).ThenInclude(x => x.ProductItemImages)
                .Where(x => x.ProductItems.Any(pi =>
                    request.MaxPrice >= (pi.Price - pi.Price * pi.Percent / 100) &&
                    request.MinPrice <= (pi.Price - pi.Price * pi.Percent / 100))).Select(x => new ProductResponse
                {
                    Id = x.Id,
                    Title = x.Title,
                    ImagePath = x.ProductItems.First(pi=>!pi.Removed).ProductItemImages.First().ImagePath,
                    TitleDescription = x.TitleDescription,
                    ProductItems = x.ProductItems.Where(pi=>!pi.Removed).Select(pi => new ProductItemResponse
                    {
                        Id = pi.Id,
                        Discount = pi.Percent,
                        Measure = pi.Measure,
                        Price = pi.Price,
                        SellingPrice = Math.Round(pi.Price - pi.Price * pi.Percent / 100,2)
                    }).ToList()
                }).ToListAsync();
        }

        public async Task<ProductItemDetailsResponse> GetProductItemDetailsAsync(int productItemId)
        {
            return await _context.ProductItems.Where(x=>x.Id == productItemId)
                .Include(x=>x.Descriptions)
                .Include(x=>x.ProductItemImages)
                .Include(x=>x.Reviews)
                .Select(x=> new ProductItemDetailsResponse
                {
                    Id = x.Id,
                    Measure = x.Measure,
                    Price = x.Price,
                    Rating = x.Reviews.Count > 0 ? x.Reviews.Average(r=>r.Rating) : 0,
                    CommentsCount = x.Reviews.Count,
                    ImagesPath = x.ProductItemImages.Select(pii=>pii.ImagePath).ToList(),
                    SellingPrice = Math.Round(x.Price - x.Price * x.Percent / 100,2),
                    VendorCode = x.VendorCode,
                    Descriptions = x.Descriptions.Select(d=>new DescriptionDetailsResponse
                    {
                        Id = d.Id,
                        Content = d.Content,
                        Title = d.Title
                    }).ToList()
                }).FirstOrDefaultAsync();
        }

        public async Task<ProductDetailsResponse> GetProductDetailsAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null || product.Removed)
                return null;
            var productItems =
                await _context.ProductItems.Where(x => x.ProductId == id && !x.Removed)
                    .Include(x=>x.Reviews)
                    .Include(x=>x.Descriptions)
                    .Include(x=>x.ProductItemImages)
                    .ToListAsync();
            return new ProductDetailsResponse
            {
                Id = product.Id,
                Title = product.Title,
                TitleDescription = product.TitleDescription,
                ProductItems = productItems.Select(pi=> new ProductItemDetailsResponse
                {
                    Id = pi.Id,
                    Measure = pi.Measure,
                    Price = pi.Price,
                    Rating = pi.Reviews.Count > 0 ? pi.Reviews.Average(r=>r.Rating) : 0,
                    CommentsCount = pi.Reviews.Count,
                    ImagesPath = pi.ProductItemImages.Select(pii=>pii.ImagePath).ToList(),
                    SellingPrice = Math.Round(pi.Price - pi.Price * pi.Percent / 100,2),
                    VendorCode = pi.VendorCode,
                    Descriptions = pi.Descriptions.Select(d=> new DescriptionDetailsResponse
                    {
                        Content = d.Content,
                        Id = d.Id,
                        Title = d.Title
                    }).ToList()
                }).ToList()
            };
        }

        private void GetParentCategoryCategories(ref List<int> categoryIds, int parentCategoryId, List<Category> categories)
        {
            var childCategories = categories.Where(x => x.ParentCategoryId == parentCategoryId).Select(x => x.Id)
                .ToList();
            categoryIds.AddRange(childCategories);
            if (childCategories.Count > 0)
            {
                foreach (var categoryId in childCategories)
                {
                    GetParentCategoryCategories(ref categoryIds,categoryId,categories);
                }
            }
        }

        //        #region CRUD products
        //        public async Task<int> CreateProduct(InpProductModel product)
        //        {
        //            if(product.ProductItems.Count()==0)
        //            {
        //                return 0;
        //            }
        //            Product prod = _mapper.Map<InpProductModel, Product>(product);
        //            prod.Removed = true;
        //            prod.PromotionId = 0;
        //            prod.Discount = product.Discount;
        //            prod.Image = "http://api.zoomag.tj/Resources/Images/Products/image.png";
        //            _context.Products.Add(prod);
        //            await Save();

        //            foreach(var item in product.ProductItems)
        //            {
        //                ProductItem prodItem = _mapper.Map<ProductItemModel, ProductItem>(item);
        //                prodItem.IsActive = true;
        //                prodItem.ProductId = prod.Id;
        //                _context.ProductItems.Add(prodItem);
        //                await Save();
        //                await CreateProductGaleries(prod.Id,prodItem.Id,item.Images);
        //            }

        //            return prod.Id;
        //        }

        //        public Task<int> UpdateProduct(UpdProductModel product)
        //        {
        //            throw new NotImplementedException();
        //        }


        //        public Task<Response> DeleteImage(int id, int productId)
        //        {
        //            throw new NotImplementedException();
        //        }

        //        public FirstProductModel FetchProductById(int id)
        //        {
        //            Product prod = _context.Products.FirstOrDefault(p => p.Id == id && p.IsActive);
        //            if (prod == null)
        //                return null;

        //            var product = _mapper.Map<Product, FirstProductModel>(prod);
        //            product.category = _context.Categories.Find(prod.CategoryId);

        //            return product;
        //        }

        //        public async Task<List<OutProductModel>> FetchProductByIds(int[] ids)
        //        {
        //            var prods = await _context.Products.Where(p => ids.Contains(p.Id) && p.IsActive).ToListAsync<Product>();
        //            if (prods.Count() == 0)
        //                return null;
        //            return _mapper.Map<List<Product>, List<OutProductModel>>(prods);
        //        }


        //        public async Task<List<OutProductModel>> FetchProducts(FetchProductsRequest request) //int rows_limit, int rows_offset, int categoryId, int brandId, int minp, int maxp, bool issale, bool isnew, bool istop, bool isrecommended)
        //        {
        //            return await _context.Products.Where(x => x.IsActive &&
        //                                                      (!request.IsNew || x.IsNew) &&
        //                                                      (!request.IsRecommended || x.IsRecommended) &&
        //                                                      (!request.IsSale || x.IsSale) &&
        //                                                      (!request.IsTop || x.IsTop) &&
        //                                                      (request.CategoriesId == null ||
        //                                                       request.CategoriesId.Contains(x.CategoryId)) &&
        //                                                      (request.BrandsId == null ||
        //                                                       request.BrandsId.Contains(x.BrandId)))
        //                .Include(x => x.ProductItems)
        //                .Select(x => new OutProductModel
        //                {
        //                    Id = x.Id,
        //                    Image = x.Image,
        //                    Name = x.Name,
        //                    SellingPrice = x.ProductItems.First().SellingPrice,
        //                    ShortDescription = x.ShortDescription
        //                }).Skip(request.Offset)
        //                .Take(request.Limit)
        //                .ToListAsync();
        //            // List<Product> products = await _context.Products
        //            //     .Where(p =>
        //            // p.IsActive &&
        //            // (categoryId != 0 ? p.CategoryId == categoryId : true) &&
        //            // (brandId != 0 ? p.BrandId == brandId : true) &&
        //            // (issale ? p.IsSale : true) &&
        //            // (isnew ? p.IsNew : true) &&
        //            // (istop ? p.IsTop : true) &&
        //            // (isrecommended ? p.IsRecommended : true))
        //            //     .Skip(rows_offset).Take(rows_limit).ToListAsync();
        //            //
        //            // return _mapper.Map<List<Product>, List<OutProductModel>>(products);
        //        }


        //        public async Task<List<OutProductModel>> FetchSales(int count)
        //        {
        //            var products = await _context.Products.Where(p => p.IsActive && p.IsSale).Take(count * 3).ToListAsync();
        //            products = products.OrderBy(x => Guid.NewGuid()).Take(count).ToList();
        //            List<OutProductModel> prods = new List<OutProductModel>();
        //            foreach (var prod in products)
        //            {
        //                prods.Add(_mapper.Map<Product, OutProductModel>(prod));
        //            }
        //            return prods;
        //        }


        //        public async Task<List<OutProductModel>> FetchTopes(int count)
        //        {
        //            var products = await _context.Products.Where(p => p.IsActive && p.IsTop).Take(count * 3).ToListAsync();
        //            products = products.OrderBy(x => Guid.NewGuid()).Take(count).ToList();
        //            List<OutProductModel> prods = new List<OutProductModel>();
        //            foreach (var prod in products)
        //            {
        //                prods.Add(_mapper.Map<Product, OutProductModel>(prod));
        //            }
        //            return prods;
        //        }

        //        public async Task<List<OutProductModel>> FetchRecommended(int count)
        //        {
        //            var products = await _context.Products.Where(p => p.IsActive && p.IsRecommended).Take(count * 3).ToListAsync();
        //            products = products.OrderBy(x => Guid.NewGuid()).Take(count).ToList();
        //            List<OutProductModel> prods = new List<OutProductModel>();
        //            foreach (var prod in products)
        //            {
        //                prods.Add(_mapper.Map<Product, OutProductModel>(prod));
        //            }
        //            return prods;
        //        }


        //        public async Task<List<OutProductModel>> FetchNew(int count)
        //        {
        //            var products = await _context.Products.Where(p => p.IsActive && p.IsNew).Take(count * 3).ToListAsync();
        //            products = products.OrderBy(x => Guid.NewGuid()).Take(count).ToList();
        //            List<OutProductModel> prods = new List<OutProductModel>();
        //            foreach (var prod in products)
        //            {
        //                prods.Add(_mapper.Map<Product, OutProductModel>(prod));
        //            }
        //            return prods;
        //        }


        //        // public async Task<int> UpdateProduct(UpdProductModel product)
        //        // {
        //        //     Product prod = await _context.Products.SingleOrDefaultAsync(p => p.Id == product.Id && p.IsActive);
        //        //     if (prod == null)
        //        //     {
        //        //         return 0;
        //        //     }
        //        //
        //        //     prod.CategoryId = await _context.Categories.FindAsync(prod.CategoryId) == null ? 0 : product.CategoryId;
        //        //
        //        //     prod.MeasureId = await _context.Measures.FindAsync(prod.MeasureId) == null ? 0 : product.MeasureId;
        //        //
        //        //     prod.BrandId = await _context.Brands.FindAsync(prod.BrandId) == null ? 0 : product.BrandId;
        //        //
        //        //     prod.NameRu = product.NameRu;
        //        //     prod.NameEn = product.NameEn;
        //        //     prod.DiscriptionRu = product.DiscriptionRu;
        //        //     prod.DiscriptionEn = product.DiscriptionEn;
        //        //     prod.ShortDiscriptionRu = product.ShortDiscriptionRu;
        //        //     prod.ShortDiscriptionEn = product.ShortDiscriptionEn;
        //        //     prod.ColorRu = product.ColorRu;
        //        //     prod.ColorEn = product.ColorEn;
        //        //     prod.Weight = product.Weight;
        //        //     prod.IsNew = product.IsNew;
        //        //     prod.IsSale = product.IsSale;
        //        //     prod.OriginalPrice = product.OriginalPrice;
        //        //     prod.SellingPrice = (product.SellingPrice != 0 && product.IsSale) ? product.SellingPrice : product.OriginalPrice;
        //        //     prod.SaleStartDate = product.SaleStartDate;
        //        //     prod.SaleEndDate = product.SaleEndDate;
        //        //     prod.Quantity = product.Quantity;
        //        //     prod.IsTop = product.IsTop;
        //        //     prod.IsRecommended = product.IsRecommended;
        //        //     prod.BrandId = product.BrandId;
        //        //     await Save();
        //        //
        //        //     return prod.Id;
        //        // }

        //        public async Task<Response> DeleteProduct(int id)
        //        {
        //            Product product = _context.Products.FirstOrDefault(p => p.Id == id);
        //            if (product != null)
        //            {
        //                var productItems = await _context.ProductItems.Where(x => x.ProductId == product.Id).Select(x=>x.Id ).ToListAsync();
        //                foreach (var productItem in productItems)
        //                {
        //                    DeleteDirectory(productItem);
        //                    await DeleteProductGaleries(productItem);
        //                }
        //                product.Image = "Resources/Images/deleted.png";
        //                product.IsActive = false;
        //                await Save();
        //                return new Response { Status = "success", Message = "Продукт успешно удален!" };
        //            }
        //            return new Response { Status = "error", Message = "Продукт не существует!" };
        //        }
        //        #endregion


        //        #region product images
        //        public async Task<string> UploadImage(int productItemId, IFormFile file)
        //        {
        //            string fName = Guid.NewGuid().ToString() + file.FileName;
        //            string path = Path.GetFullPath("Resources/Images/Products/" + productItemId);
        //            if (!Directory.Exists(path))
        //            {
        //                Directory.CreateDirectory(path);
        //            }
        //            path = Path.Combine(path, fName);
        //            using (var stream = new FileStream(path, FileMode.Create))
        //            {
        //                await file.CopyToAsync(stream);
        //            }
        //            return "http://api.zoomag.tj/Resources/Images/Products/" + productItemId + "/" + fName;
        //        }

        //        public async Task CreateProductGaleries(int productId,int productItemId, IFormFile[] images)
        //        {
        //            for (int i = 1; i <= images.Length; i++)
        //            {
        //                string fileName = await UploadImage(productItemId, images[i - 1]);
        //                if (i == 1)
        //                {
        //                    Product product = await _context.Products.FindAsync(productId);
        //                    if (product != null)
        //                    {
        //                        if (String.IsNullOrEmpty(product.Image) || product.Image == "http://api.zoomag.tj/Resources/Images/Products/image.png")
        //                        {
        //                            product.Image = fileName;
        //                            await Save();
        //                            continue;
        //                        }
        //                    }
        //                }
        //                _context.ProductGaleries.Add(
        //                    new ProductGalery
        //                    {
        //                        ProductItemId = productItemId,
        //                        Image = fileName
        //                    });
        //            }
        //            await Save();
        //        }

        //        public async Task<Response> SetMainImage(int productid, int imageid)
        //        {
        //            var product = await _context.Products.FindAsync(productid);
        //            if (product == null)
        //            {
        //                return new Response { Status = "error", Message = "Товар не найден!" };
        //            }
        //            var galery = await _context.ProductGaleries.FindAsync(imageid);
        //            if (galery == null)
        //            {
        //                return new Response { Status = "error", Message = "Фото не найдено!" };
        //            }
        //            string img = product.Image;
        //            product.Image = galery.Image;
        //            galery.Image = img;
        //            await Save();
        //            return new Response { Status = "success", Message = "Фото успешно присвоен!" }; ;
        //        }

        //        private async Task DeleteProductGaleries(int productItemId)
        //        {
        //            var galeries = await _context.ProductGaleries.Where(p => p.ProductItemId == productItemId).ToListAsync();
        //            _context.ProductGaleries.RemoveRange(galeries);
        //            await Save();
        //            return;
        //        }

        //        // public async Task<List<ProductImagesModel>> FetchProductGaleriesByProductId(int productId)
        //        // {
        //        //     var res = await _context.ProductGaleries.Where(p => p.ProductId == productId).ToListAsync();
        //        //     return _mapper.Map<List<ProductGalery>, List<ProductImagesModel>>(res);
        //        // }

        //        private void DeleteDirectory(int productId)
        //        {
        //            string path = "Resources/Images/Products/" + productId;
        //            if (Directory.Exists(path))
        //                Directory.Delete(path, true);
        //        }

        //        private void DeleteImage(string path)
        //        {
        //            FileInfo fileInfo = new FileInfo(path);
        //            if (fileInfo.Exists)
        //            {
        //                fileInfo.Delete();
        //            }
        //        }


        //        // public async Task<Response> DeleteImage(int id, int productId)
        //        // {
        //        //     if (id != 0)
        //        //     {
        //        //         var galery = _context.ProductGaleries.FirstOrDefault(p => p.Id == id);
        //        //         if (galery != null)
        //        //         {
        //        //             DeleteImage(galery.Image);
        //        //             await DeleteProductGalery(id);
        //        //             return new Response { Status = "success", Message = "Фотография успешно удалено!" };
        //        //         }
        //        //     }
        //        //     else
        //        //     {
        //        //         var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == productId);
        //        //         if (product != null)
        //        //         {
        //        //             DeleteImage(product.Image);
        //        //             var galery = _context.ProductGaleries.FirstOrDefault(p => p.ProductId == productId);
        //        //             if (galery != null)
        //        //             {
        //        //                 product.Image = galery.Image;
        //        //                 await Save();
        //        //                 await DeleteProductGalery(galery.Id);
        //        //             }
        //        //             else
        //        //             {
        //        //                 product.Image = null;
        //        //                 await Save();
        //        //             }
        //        //             return new Response { Status = "success", Message = "Фотография успешно удалено!" };
        //        //         }
        //        //     }
        //        //     return new Response { Status = "error", Message = "Фотография не найдена!" };
        //        // }

        //        private async Task<int> DeleteProductGalery(int id)
        //        {
        //            var galery = await _context.ProductGaleries.FirstOrDefaultAsync(p => p.Id == id);
        //            if (galery != null)
        //            {
        //                _context.ProductGaleries.Remove(galery);
        //            }
        //            return await _context.SaveChangesAsync();
        //        }

        //        #endregion


        //        public Task<List<ProductImagesModel>> FetchProductGaleriesByProductId(int productId)
        //        {
        //            throw new NotImplementedException();
        //        }

        //        public async Task<int> Save()
        //        {
        //            return await _context.SaveChangesAsync();
        //        }

        //        public async Task<int> CountProducts(int categoryId, int brandId, int minp, int maxp, bool issale, bool isnew, bool istop, bool isrecommended)
        //        {
        //            return await _context.Products
        //                 .Where(p =>
        //             p.IsActive &&
        //             (categoryId != 0 ? p.CategoryId == categoryId : true) &&
        //             (brandId != 0 ? p.BrandId == brandId : true) &&
        //             (issale ? p.IsSale : true) &&
        //             (isnew ? p.IsNew : true) &&
        //             (istop ? p.IsTop : true) &&
        //             (isrecommended ? p.IsRecommended : true)).CountAsync();

        //        }

        //        public async Task<int> SearchCount(int categoryId, string q)
        //        {
        //            return await _context.Products
        //                .Where(p =>
        //                p.IsActive &&
        //                (categoryId != 0 ? p.CategoryId == categoryId : true) &&
        //                (p.Name.Contains(q)))
        //                .CountAsync();
        //        }

        //        public async Task<List<OutProductModel>> Search(int rows_limit, int rows_offset, int categoryId, string q)
        //        {
        //            return await _context.Products
        //                    .Where(p => p.IsActive
        //                    && (categoryId != 0 ? p.CategoryId == categoryId : true)
        //                    && (p.Name.Contains(q)))
        //                    .Skip(rows_offset)
        //                    .Take(rows_limit)
        //                    .Include(x=>x.ProductItems)
        //                    .Select(x=> new OutProductModel()
        //                    {
        //                        Id = x.Id,
        //                        Image = x.Image,
        //                        Name = x.Name,
        //                        SellingPrice = x.ProductItems.FirstOrDefault().SellingPrice,
        //                        ShortDescription = x.ShortDescription
        //                    })
        //                    .ToListAsync();
        //        }

        //        public Task CreateProductGaleries(int productId, IFormFile[] images)
        //        {
        //            throw new NotImplementedException();
        //        }
    }
}