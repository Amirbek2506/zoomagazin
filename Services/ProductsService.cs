using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ZooMag.Data;
using ZooMag.DTOs;
using ZooMag.DTOs.Category;
using ZooMag.DTOs.Description;
using ZooMag.DTOs.Product;
using ZooMag.DTOs.ProductItem;
using ZooMag.Entities;
using ZooMag.Services.Interfaces;
using ZooMag.ViewModels;

namespace ZooMag.Services
{
    public class ProductsService : IProductsService
    {
        private readonly ApplicationDbContext _context;
        private readonly IFileService _fileService;

        public ProductsService(ApplicationDbContext context, IFileService fileService)
        {
            _context = context;
            _fileService = fileService;
        }

        public async Task<int> CreateAsync(CreateProductRequest request)
        {
            // if (request.ProductItems.Count == 0)
            //     return new Response { Message = "", Status = "error" };
            // Dictionary<string, List<string>> productItemImages = new Dictionary<string, List<string>>();
            //
            // string emptyFilePath = Path.GetFullPath("Resources/Images/Products/image.png");
            //
            // foreach(var productItem in request.ProductItems)
            // {
            //     var productImages = await _fileService.AddProductItemFilesASync(productItem.Images);
            //     productItemImages.Add(productItem.VendorCode, productImages.Count > 0 ? productImages : new List<string> { emptyFilePath });
            // }
            //
            // Dictionary<string, List<Description>> productItemDescriptions = request.ProductItems.ToDictionary(
            //     x => x.VendorCode, x => x.Descriptions.Select(d => new Description
            //     {
            //         Content = d.Content,
            //         Title = d.Title
            //     }).ToList());

            var product = new Product
            {
                BrandId = request.BrandId,
                CategoryId = request.CategoryId,
                CreateDate = DateTime.Now,
                Title = request.Title,
                TitleDescription = request.TitleDescription,
                Removed = false,
                ProductSpecificFilters = request.SpecificFiltersId?.Select(x=>new ProductSpecificFilter
                {
                    SpecificFilterId = x
                }).ToList()
                // ProductItems = request.ProductItems.Select(x => new ProductItem
                // {
                //     Measure = x.Measure,
                //     Percent = x.Percent,
                //     Price = x.Price,
                //     VendorCode = x.VendorCode,
                //     ProductItemImages = productItemImages[x.VendorCode].Select(pi => new ProductItemImage
                //     {
                //         ImagePath = pi
                //     }).ToList(),
                //     Descriptions = productItemDescriptions[x.VendorCode]
                // }).ToList()
            };

            await _context.Products.AddAsync(product);

            await _context.SaveChangesAsync();
            return product.Id;
            // return new
            // {
            //     Id = product.Id,
            //     Title = product.Title,
            //     TitleDescription = product.TitleDescription
            // };
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

            string deletedFilePath = "Resources/no-image.png";

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

        public async Task<GenericResponse<List<MostPopularProductResponse>>> GetMostPopularAsync(GenericPagedRequest<int> request)
        {
            var categories = await _context.Categories.ToListAsync();
            List<int> categoryIds = new List<int> {request.Query};
            GetParentCategoryCategories(ref categoryIds,request.Query,categories);

            return new()
            {
                Payload = await _context.Products.Where(x => categoryIds.Contains(x.CategoryId) && !x.Removed)
                    .OrderByDescending(x=>x.CreateDate)
                    .Skip(request.Offset).Take(request.Limit)
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
                    }).ToListAsync(),
                Count = await _context.Products.Where(x => categoryIds.Contains(x.CategoryId) && !x.Removed).CountAsync()
            };
        }

        public async Task<SearchResponse> SearchAsync(GenericPagedRequest<string> request)
        {
            List<SearchCategoryResponse> searchCategoryResponses = new List<SearchCategoryResponse>();

            var categories = await _context.Categories.ToListAsync();
            
            var searchCategories = categories.Where(x => x.Name.ToLower().Contains(request.Query.ToLower()))
                .ToList();

            for (int i = 0; i < searchCategories.Count; i++)
            {
                int? parentCategoryId = searchCategories[i].ParentCategoryId;

                string categoryName = searchCategories[i].Name;
                
                while (parentCategoryId != null)
                {
                    var parentCategory = categories.First(x => x.Id == parentCategoryId);

                    categoryName = $"{parentCategory.Name} / {categoryName}"; 

                    parentCategoryId = parentCategory.ParentCategoryId;
                }
                
                searchCategoryResponses.Add(new SearchCategoryResponse
                {
                    Id = searchCategories[i].Id,
                    Name = categoryName
                });
            }
            
            var products = await _context.Products.Where(x=>!x.Removed).Include(x => x.ProductItems).ThenInclude(x => x.ProductItemImages)
                .Where(x =>x.Title.ToLower().Contains(request.Query.ToLower()) ||
                            x.ProductItems.Any(pi => pi.VendorCode.ToLower().Contains(request.Query.ToLower())))
                .OrderByDescending(x=>x.Id)
                .Skip(request.Offset)
                .Take(request.Limit)
                .Select(x => new SearchProductResponse
                {
                    ProductId = x.Id,
                    Price = x.ProductItems.First(pi=>!pi.Removed).Price,
                    Title = x.Title,
                    ImagePath = x.ProductItems.First(pi=>!pi.Removed).ProductItemImages.First().ImagePath
                }).ToListAsync();
            
            return new()
            {
                Categories = searchCategoryResponses, 
                Products = products, 
                ProductsCount = products.Count
            };
        }

        public async Task<GenericResponse<List<ProductResponse>>> GetAllAsync(GenericPagedRequest<string> request)
        {
            return new ()
            {
                Payload = await _context.Products.Where(x => !x.Removed && (string.IsNullOrEmpty(request.Query) || x.Title.Contains(request.Query)))
                    .OrderByDescending(x=>x.Id)
                    .Skip(request.Offset)
                    .Take(request.Limit)
                    .Include(x => x.ProductItems)
                    .ThenInclude(x => x.ProductItemImages)
                    .Include(x=>x.Brand)
                    .Include(x=>x.Category)
                    .Select(x => new ProductResponse
                    {
                        Id = x.Id,
                        Title = x.Title,
                        TitleDescription = x.TitleDescription,
                        ImagePath = x.ProductItems.First(pi => !pi.Removed).ProductItemImages.First().ImagePath,
                        BrandId = x.BrandId,
                        CategoryId = x.CategoryId,
                        BrandName = x.Brand.Name,
                        CategoryName = x.Category.Name,
                        ProductItems = x.ProductItems.Where(pi => !pi.Removed).Select(pi => new ProductItemResponse
                        {
                            Discount = pi.Percent,
                            Measure = pi.Measure,
                            Price = pi.Price,
                            SellingPrice = Math.Round(pi.Price - pi.Price * pi.Percent / 100, 2),
                            Id = pi.Id
                        }).ToList()
                    }).ToListAsync(),
                Count = await _context.Products.Where(x => !x.Removed && (string.IsNullOrEmpty(request.Query) || x.Title.Contains(request.Query))).CountAsync()
            };
        }

        public async Task<List<WishListProductItemResponse>> GetWishListAsync(string key)
        {
            return await _context.Wishlists.Where(x => x.UserId == key).Include(x => x.ProductItem)
                .ThenInclude(x => x.Product)
                .Include(x => x.ProductItem).ThenInclude(x => x.ProductItemImages)
                .Select(x => new WishListProductItemResponse
                {
                    ProductId = x.ProductItem.Product.Id,
                    ProductItemId = x.ProductItemId,
                    Price = x.ProductItem.Price,
                    Title = x.ProductItem.Product.Title,
                    Measure = x.ProductItem.Measure,
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
            var productItem = await _context.ProductItems.FindAsync(productItemId);
            if (productItem == null)
                return new Response {Status = "error", Message = "Продукт не найден"};
            wishlist = new Wishlist
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

        public async Task<Response> AddToBasketAsync(string key,AddToBasketRequest request)
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
                    Count = request.Count < 1 ? 1 : request.Count,
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
            
            var newWishlists = wishListsId.Except(oldWishlists).Select(x => new Wishlist {UserId = newKey, ProductItemId = x}).ToList();
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
            if (basketProduct == null)
                return new Response {Message = "Не найден", Status = "error"};
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
            if (basketProduct.Count == 1)
                _context.Baskets.Remove(basketProduct);
            else
            {
                basketProduct.Count--;
                _context.Baskets.Update(basketProduct);
            }
            await _context.SaveChangesAsync();
            return new Response {Status = "success", Message = "Успешно"};
        }

        public async Task<GenericResponse<List<ProductResponse>>> GetFilteredProductsAsync(GenericPagedRequest<ProductFiltersRequest> request)
        {
            if (request.Query.MaxPrice < request.Query.MinPrice)
                return new GenericResponse<List<ProductResponse>>
                {
                    Payload = new List<ProductResponse>(),
                    Count = 0
                };
            var categories = await _context.Categories
                .Include(x=>x.CategoryFilters)
                .Where(x=>x.CategoryFilters.Any(cf=>request.Query.FiltersId.Contains(cf.FilterId)))
                .ToListAsync();
            List<int> categoryIds = new List<int> { request.Query.CategoryId};
            GetParentCategoryCategories(ref categoryIds,request.Query.CategoryId,categories);

            var queryableProducts = _context.Products
                .Include(x=>x.ProductSpecificFilters)
                .Where(x => !x.Removed &&
                            (request.Query.SpecificFiltersId == null || x.ProductSpecificFilters.Any(sf=> request.Query.SpecificFiltersId.Contains(sf.SpecificFilterId))) &&// request.Query.SpecificFiltersId.Any(sf=>x.ProductSpecificFilters.Contains(sf))) &&
                            (request.Query.CategoryId == 0 || categoryIds.Contains(x.CategoryId)) &&
                            (request.Query.BrandsId == null || request.Query.BrandsId.Contains(x.BrandId)))
                .Include(x => x.ProductItems).ThenInclude(x => x.ProductItemImages)
                .Where(x => x.ProductItems.Any(pi =>
                    request.Query.MaxPrice >= (pi.Price - pi.Price * pi.Percent / 100) &&
                    request.Query.MinPrice <= (pi.Price - pi.Price * pi.Percent / 100)));

            if (request.Query.SortType == 1) //"Алфавиту: от А до Я"
                queryableProducts = queryableProducts.OrderBy(x => x.Title);
            if(request.Query.SortType == 2) //"Алфавиту: от Я до А"
                queryableProducts = queryableProducts.OrderByDescending(x => x.Title);
            if(request.Query.SortType == 3) //"Новизне"
                queryableProducts = queryableProducts.OrderByDescending(x => x.CreateDate);
            if(request.Query.SortType == 4) //"Популярности"
                queryableProducts = queryableProducts.OrderByDescending(x => x.ProductItems.Max(pi => pi.Reviews.Average(c => c.Rating)));
            if(request.Query.SortType == 5) //"Цена по возрастанию"
                queryableProducts = queryableProducts.OrderBy(x => x.ProductItems.Min(pi => pi.Price));
            if(request.Query.SortType == 6) //"Цена по убыванию"
                queryableProducts = queryableProducts.OrderByDescending(x => x.ProductItems.Min(pi=> pi.Price));

            return new GenericResponse<List<ProductResponse>>
            {
                Payload = await queryableProducts.Skip(request.Offset)
                    .Take(request.Limit)
                    .Select(x => new ProductResponse
                    {
                        Id = x.Id,
                        Title = x.Title,
                        ImagePath = x.ProductItems.First(pi => !pi.Removed).ProductItemImages.First().ImagePath,
                        TitleDescription = x.TitleDescription,
                        ProductItems = x.ProductItems.Where(pi => !pi.Removed).Select(pi => new ProductItemResponse
                        {
                            Id = pi.Id,
                            Discount = pi.Percent,
                            Measure = pi.Measure,
                            Price = pi.Price,
                            SellingPrice = Math.Round(pi.Price - pi.Price * pi.Percent / 100, 2)
                        }).ToList()
                    }).ToListAsync(),
                Count = await queryableProducts.CountAsync()
            };
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
                    Percent = x.Percent,
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
            var category = await _context.Categories.FindAsync(product.CategoryId);
            var brand = await _context.Brands.FindAsync(product.BrandId);
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
                BrandId = product.BrandId,
                CategoryId = product.CategoryId,
                BrandName = brand.Name,
                CategoryName = category.Name,
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

        public async Task<GenericResponse<List<ProductResponse>>> GetProductsByBrandIdAsync(GenericPagedRequest<int> request)
        {
            return new()
            {
                Payload = await _context.Products.Where(x => !x.Removed && x.BrandId == request.Query)
                    .OrderByDescending(x=>x.Id)
                    .Skip(request.Offset)
                    .Take(request.Limit)
                    .Include(x => x.ProductItems)
                    .ThenInclude(x => x.ProductItemImages)
                    .Select(x => new ProductResponse
                    {
                        Id = x.Id,
                        Title = x.Title,
                        TitleDescription = x.TitleDescription,
                        ImagePath = x.ProductItems.First(pi => !pi.Removed).ProductItemImages.First().ImagePath,
                        ProductItems = x.ProductItems.Where(pi => !pi.Removed).Select(pi => new ProductItemResponse
                        {
                            Id = pi.Id,
                            Discount = pi.Percent,
                            Measure = pi.Measure,
                            Price = pi.Price,
                            SellingPrice = Math.Round(pi.Price - pi.Price * pi.Percent / 100, 2)
                        }).ToList()
                    }).ToListAsync(),
                Count = await _context.Products.Where(x => !x.Removed && x.BrandId == request.Query).CountAsync()
            };
        }

        public async Task<List<SelectOptionProductResponse>> GetProductsForSelectOptionAsync()
        {
            return await _context.Products.Select(x => new SelectOptionProductResponse
            {
                Id = x.Id,
                Name = x.Title
            }).ToListAsync();
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
    }
}