using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using ZooMag.Data;
using ZooMag.Mapping;
using ZooMag.Models;
using ZooMag.Services.Interfaces;
using ZooMag.ViewModels;

namespace ZooMag.Services
{
    public class ProductsService : IProductsService
    {
        private readonly ApplicationDbContext _context;

        private readonly IMapper _mapper;

        public ProductsService(ApplicationDbContext context)
        {
            _context = context;
            _mapper = new MapperConfiguration(x => x.AddProfile<GeneralProfile>()).CreateMapper();
        }


        #region CRUD products
        public int CreateProduct(ProductModel product)
        {
            Product prod = new Product
            {
                NameRu = product.Name,
                DiscriptionRu = product.Discription,
                ShortDiscriptionRu = product.ShortDiscription,
                CategoryId = product.CategoryId,
                MeasureId = product.MeasureId,
                ColorRu = product.Color,
                Weight = product.Weight,
                IsNew = product.IsNew,
                IsSale = product.IsSale,
                OriginalPrice = product.OriginalPrice,
                SellingPrice = product.SellingPrice,
                SaleStartDate = product.SaleStartDate,
                SaleEndDate = product.SaleEndDate,
                Quantity = product.Quantity,
                IsActive = true
            };
            _context.Products.Add(prod);
            _context.SaveChanges();

            return prod.Id;
        }


        public ProductModel FetchProductById(int id)
        {
            Product prod = _context.Products.FirstOrDefault(p => p.Id == id && p.IsActive);
            if (prod == null)
                return null;
            return new ProductModel
            {
                Id = prod.Id,
                CategoryId = prod.CategoryId,
                Color = prod.ColorRu,
                Discription = prod.DiscriptionRu,
                IsNew = prod.IsNew,
                IsSale = prod.IsSale,
                MeasureId = prod.MeasureId,
                Name = prod.NameRu,
                OriginalPrice = prod.OriginalPrice,
                Quantity = prod.Quantity,
                Image = prod.Image,
                SaleEndDate = prod.SaleEndDate,
                SaleStartDate = prod.SaleStartDate,
                SellingPrice = prod.SellingPrice,
                ShortDiscription = prod.ShortDiscriptionRu,
                Weight = prod.Weight,
            };
        }


        public async Task<List<ProductModel>> FetchProducts()
        {
            List<Product> products = await _context.Products.Where(p => p.IsActive).ToListAsync();
            List<ProductModel> prods = new List<ProductModel>();
            foreach (var prod in products)
            {
                prods.Add(new ProductModel
                {
                    Id = prod.Id,
                    CategoryId = prod.CategoryId,
                    Color = prod.ColorRu,
                    Discription = prod.DiscriptionRu,
                    IsNew = prod.IsNew,
                    IsSale = prod.IsSale,
                    MeasureId = prod.MeasureId,
                    Name = prod.NameRu,
                    OriginalPrice = prod.OriginalPrice,
                    Quantity = prod.Quantity,
                    Image = prod.Image,
                    SaleEndDate = prod.SaleEndDate,
                    SaleStartDate = prod.SaleStartDate,
                    SellingPrice = prod.SellingPrice,
                    ShortDiscription = prod.ShortDiscriptionRu,
                    Weight = prod.Weight,
                });
            }
            return prods;
        }


        public async Task<int> UpdateProduct(ProductModel product)
        {
            Product prod = await _context.Products.SingleOrDefaultAsync(p => p.Id == product.Id && p.IsActive);
            if (prod == null)
            {
                return 0;
            }
            prod.NameRu = product.Name;
            prod.DiscriptionRu = product.Discription;
            prod.ShortDiscriptionRu = product.ShortDiscription;
            prod.MeasureId = product.MeasureId;
            prod.ColorRu = product.Color;
            prod.Weight = product.Weight;
            prod.IsNew = product.IsNew;
            prod.IsSale = product.IsSale;
            prod.OriginalPrice = product.OriginalPrice;
            prod.SellingPrice = product.SellingPrice;
            prod.SaleStartDate = product.SaleStartDate;
            prod.SaleEndDate = product.SaleEndDate;
            prod.Quantity = product.Quantity;
            _context.Products.Update(prod);
            await Save();

            return prod.Id;
        }

        public async Task<Response> DeleteProduct(int id)
        {
            Product product = _context.Products.FirstOrDefault(p => p.Id == id);
            if (product != null)
            {
                DeleteDirectory(id);
                await DeleteProductGaleries(id);
                await DeleteProductSizes(id);
                product.Image = "Resources/Images/deleted.png";
                product.IsActive = false;
                return new Response { Status = "success", Message = "Продукт успешно удален!" };
            }
                return new Response { Status = "error", Message = "Продукт не существует!" };
        }
        #endregion


        #region product sizes
        public async Task<List<int>> CreateSizes(List<string> sizes)
        {
            List<int> sizeIds = new List<int>();
            foreach (var item in sizes)
            {
                if(String.IsNullOrEmpty(item))
                {
                    continue;
                }
                Size size = await _context.Sizes.FirstOrDefaultAsync(p => p.Title == item);
                if (size == null)
                {
                    size = new Size();
                    size.Title = item;
                    _context.Sizes.Add(size);
                    await Save();
                }
                sizeIds.Add(size.Id);
            }
            return sizeIds;
        }

        public async Task CreateProductSizes(int productId, List<int> sizeIds)
        {
            foreach (int sizeid in sizeIds)
            {
                await _context.ProductSizes.AddAsync(new ProductSize { ProductId = productId, SizeId = sizeid });
            }
            await Save();
            return;
        }

        public async Task DeleteProductSizes(int productId)
        {
            var sizes = await _context.ProductSizes.Where(p => p.ProductId == productId).ToListAsync();
            _context.ProductSizes.RemoveRange(sizes);
            await Save();
            return;
        }

        public async Task<List<ProductSize>> FetchProductSizesByProductId(int productId)
        {
            return await _context.ProductSizes.Where(p => p.ProductId == productId).ToListAsync();
        }

        public async Task<List<SizeModel>> FetchSizesByProductId(int productId)
        {
            List<Size> sizes = new List<Size>();
            List<ProductSize> ProductSize = await FetchProductSizesByProductId(productId);
            foreach (var item in ProductSize)
            {
                Size size = await _context.Sizes.Where(s => s.Id == item.SizeId).FirstOrDefaultAsync();
                if (size != null)
                    sizes.Add(size);
            }
            return _mapper.Map<List<Size>, List<SizeModel>>(sizes); ;
        }

        #endregion


        #region product images
        public async Task<string> UploadImage(int productId, IFormFile file)
        {
            string fName = Guid.NewGuid().ToString() + file.FileName;
            string path = Path.GetFullPath("Resources/Images/Products/" + productId);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            path = Path.Combine(path, fName);
            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            return fName;
        }

        public async Task CreateProductGaleries(int productId, IFormFile[] images)
        {
            for (int i = 1; i <= images.Length; i++)
            {
                string fileName = await UploadImage(productId, images[i - 1]);
                if (i == 1)
                {
                    Product product = _context.Products.FirstOrDefault(p => p.Id == productId);
                    if (product != null)
                        product.Image = "Resources/Images/Products/"+product.Id +"/"+ fileName;
                    await Save();
                    continue;
                }
                _context.ProductGaleries.Add(
                    new ProductGalery
                    {
                        ProductId = productId,
                        Image = "Resources/Images/Products/" + productId + "/" + fileName
                    });
            }
            await Save();
            return;
        }
        private async Task DeleteProductGaleries(int productId)
        {
            var galeries = await _context.ProductGaleries.Where(p => p.ProductId == productId).ToListAsync();
                _context.ProductGaleries.RemoveRange(galeries);
            await Save();
            return;
        }

        public async Task<List<ProductImagesModel>> FetchProductGaleriesByProductId(int productId)
        {
            var res = await _context.ProductGaleries.Where(p => p.ProductId == productId).ToListAsync();
            return _mapper.Map<List<ProductGalery>, List<ProductImagesModel>>(res);
        }

        public void DeleteDirectory(int productId)
        {
            string path = "Resources/Images/Products/" + productId;
            if (Directory.Exists(path))
                Directory.Delete(path, true);
        }


        #endregion

        public async Task<int> Save()
        {
            return await _context.SaveChangesAsync();
        }

        public int CountProducts()
        {
            return _context.Products.Count();
        }
    }
}