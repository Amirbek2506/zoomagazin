﻿using System;
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
using ZooMag.Models.ViewModels.Products;
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
        public async Task<int> CreateProduct(InpProductModel product)
        {
            Product prod = _mapper.Map<InpProductModel, Product>(product);
            if (await _context.Categories.FindAsync(prod.CategoryId)==null)
            {
                prod.CategoryId = 0;
            }
            prod.SellingPrice = (product.SellingPrice != 0 && product.IsSale) ? product.SellingPrice : product.OriginalPrice;
            prod.IsActive = true;
            prod.Image = "https://u1190523.plsk.regruhosting.ru/Resources/Images/Products/image.png";
            _context.Products.Add(prod);
            await Save();
            return prod.Id;
        }


        public FirstProductModel FetchProductById(int id)
        {
            Product prod = _context.Products.FirstOrDefault(p => p.Id == id && p.IsActive);
            if (prod == null)
                return null;

            var product = _mapper.Map<Product, FirstProductModel>(prod);
            product.measure = _context.Measures.Find(prod.MeasureId);
            product.category = _context.Categories.Find(prod.CategoryId);

            return product;
        }
        
        public async Task<List<OutProductModel>> FetchProductByIds(int[] ids)
        {
            var prods = await _context.Products.Where(p => ids.Contains(p.Id)&& p.IsActive).ToListAsync<Product>();
            if (prods.Count()==0)
                return null;
            return _mapper.Map<List<Product>,List<OutProductModel>>(prods);
        }


        public async Task<List<OutProductModel>> FetchProducts(int rows_limit, int rows_offset,int categoryId, int minp, int maxp,bool issale, bool isnew)
        {
            List<Product> products = new List<Product>();
           
            if(minp!=maxp)
            {
                if (categoryId != 0)
                {
                    products = await _context.Products.Where(p => p.IsActive && p.CategoryId == categoryId && (issale ? p.IsSale : true) && (isnew ? p.IsNew : true) && p.SellingPrice >= minp && p.SellingPrice <= maxp).Skip(rows_offset).Take(rows_limit).ToListAsync();
                }
                else
                {
                    products = await _context.Products.Where(p => p.IsActive && (issale ? p.IsSale : true) && (isnew ? p.IsNew : true) && p.SellingPrice >= minp && p.SellingPrice <= maxp).Skip(rows_offset).Take(rows_limit).ToListAsync();
                }
            }
            else
            {
                if (categoryId != 0)
                {
                    products = await _context.Products.Where(p => p.IsActive && p.CategoryId == categoryId && (issale ? p.IsSale : true) && (isnew ? p.IsNew : true)).Skip(rows_offset).Take(rows_limit).ToListAsync();
                }
                else
                {
                    products = await _context.Products.Where(p => p.IsActive && (issale ? p.IsSale : true) && (isnew ? p.IsNew : true)).Skip(rows_offset).Take(rows_limit).ToListAsync();
                }
            }
            
            List<OutProductModel> prods = new List<OutProductModel>();
            foreach (var prod in products)
            {
                prods.Add(_mapper.Map<Product, OutProductModel>(prod));
            }
            return prods;
        }

         public async Task<List<OutProductModel>> FetchSales(int count)
         {
            var products = await _context.Products.Where(p => p.IsActive && p.IsSale).OrderBy(arg => Guid.NewGuid()).Take(count).ToListAsync();
            List<OutProductModel> prods = new List<OutProductModel>();
            foreach (var prod in products)
            {
                prods.Add(_mapper.Map<Product, OutProductModel>(prod));
            }
            return prods;
         }

        
         public async Task<List<OutProductModel>> FetchNew(int count)
         {
            var products = await _context.Products.Where(p => p.IsActive && p.IsNew).OrderBy(arg => Guid.NewGuid()).Take(count).ToListAsync();
            List<OutProductModel> prods = new List<OutProductModel>();
            foreach (var prod in products)
            {
                prods.Add(_mapper.Map<Product, OutProductModel>(prod));
            }
            return prods;
         }


        public async Task<int> UpdateProduct(UpdProductModel product)
        {
            Product prod = await _context.Products.SingleOrDefaultAsync(p => p.Id == product.Id && p.IsActive);
            if (prod == null)
            {
                return 0;
            }
            if (await _context.Categories.FindAsync(prod.CategoryId) == null)
            {
                prod.CategoryId = 0;
            }else
            {
                prod.CategoryId = product.CategoryId;
            }

            if (await _context.Measures.FindAsync(prod.MeasureId) == null)
            {
                prod.MeasureId = 0;
            }
            else
            {
                prod.MeasureId = product.MeasureId;
            }


            prod.NameRu = product.NameRu;
            prod.NameEn = product.NameEn;
            prod.DiscriptionRu = product.DiscriptionRu;
            prod.DiscriptionEn = product.DiscriptionEn;
            prod.ShortDiscriptionRu = product.ShortDiscriptionRu;
            prod.ShortDiscriptionEn = product.ShortDiscriptionEn;
            prod.ColorRu = product.ColorRu;
            prod.ColorEn = product.ColorEn;
            prod.Weight = product.Weight;
            prod.IsNew = product.IsNew;
            prod.IsSale = product.IsSale;
            prod.OriginalPrice = product.OriginalPrice;
            prod.SellingPrice = (product.SellingPrice!=0 && product.IsSale)?product.SellingPrice:product.OriginalPrice;
            prod.SaleStartDate = product.SaleStartDate;
            prod.SaleEndDate = product.SaleEndDate;
            prod.Quantity = product.Quantity;
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
                await Save();
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


        public async Task<Response> DeleteProductSize(int productId,int sizeId)
        {
            var productSize = await _context.ProductSizes.FirstOrDefaultAsync(p=>p.ProductId == productId && p.SizeId == sizeId);
            if(productSize!=null)
            {
                _context.ProductSizes.Remove(productSize);
                await Save();
                return new Response {Status = "success",Message = "Размер успешно удален!" };
            }
            return new Response {Status = "error", Message = "Размер не найден!"};
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
            return "https://u1190523.plsk.regruhosting.ru/Resources/Images/Products/" + productId + "/" + fName;
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
                    {
                        if(String.IsNullOrEmpty(product.Image) || product.Image == "https://u1190523.plsk.regruhosting.ru/Resources/Images/Products/image.png")
                        {
                            product.Image = fileName;
                            await Save();
                            continue;
                        }
                    }
                }
                _context.ProductGaleries.Add(
                    new ProductGalery
                    {
                        ProductId = productId,
                        Image = fileName
                    });
            }
            await Save();
            return;
        }

        public async Task<Response> SetMainImage(int productid,int imageid)
        {
            var product = await _context.Products.FindAsync(productid);
            if(product==null)
            {
                return new Response { Status = "error",Message = "Товар не найден!"};
            }
            var galery = await _context.ProductGaleries.FindAsync(imageid);
            if (galery == null)
            {
                return new Response { Status = "error", Message = "Фото не найдено!" };
            }
            string img = product.Image;
            product.Image = galery.Image;
            galery.Image = img;
            await Save();
            return new Response {Status = "success",Message = "Фото успешно присвоен!" }; ;
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

        private void DeleteDirectory(int productId)
        {
            string path = "Resources/Images/Products/" + productId;
            if (Directory.Exists(path))
                Directory.Delete(path, true);
        }

        private void DeleteImage(string path)
        {
            FileInfo fileInfo = new FileInfo(path);
            if (fileInfo.Exists)
            {
                fileInfo.Delete();
            }
        }


        public async Task<Response> DeleteImage(int id, int productId)
        {
            if (id != 0)
            {
                var galery = _context.ProductGaleries.FirstOrDefault(p => p.Id == id);
                if(galery!=null)
                {
                    DeleteImage(galery.Image);
                    await DeleteProductGalery(id);
                    return new Response { Status = "success", Message = "Фотография успешно удалено!" };
                }
            }
            else
            {
                var product = await _context.Products.FirstOrDefaultAsync(p=>p.Id == productId);
                if (product!=null)
                {
                    DeleteImage(product.Image);
                    var galery = _context.ProductGaleries.FirstOrDefault(p => p.ProductId == productId);
                    if (galery != null)
                    {
                        product.Image = galery.Image;
                        await Save();
                        await DeleteProductGalery(galery.Id);
                    }else
                    {
                        product.Image = null;
                        await Save();
                    }
                    return new Response { Status = "success", Message = "Фотография успешно удалено!" };
                }
            }
            return new Response { Status = "error", Message = "Фотография не найдена!" };
        }

        private async Task<int> DeleteProductGalery(int id)
        {
            var galery = await _context.ProductGaleries.FirstOrDefaultAsync(p=>p.Id == id);
            if(galery!=null)
            {
                _context.ProductGaleries.Remove(galery);
            }
            return await _context.SaveChangesAsync();
        }

        #endregion

        public async Task<int> Save()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task<int> CountProducts(int categoryId,int minp, int maxp, bool issale, bool isnew)
        {
            if (minp != maxp)
            {
                if (categoryId != 0)
                {
                    return await _context.Products.Where(p => p.IsActive && p.CategoryId == categoryId && (issale ? p.IsSale : true) && (isnew ? p.IsNew : true) && p.SellingPrice >= minp && p.SellingPrice <= maxp).CountAsync();
                }
                else
                {
                    return await _context.Products.Where(p => p.IsActive &&(issale ? p.IsSale : true)&& (isnew ? p.IsNew : true) && p.SellingPrice >= minp && p.SellingPrice <= maxp).CountAsync();
                }
            }
            if (categoryId != 0)
            {
                return await _context.Products.Where(p => p.IsActive && p.CategoryId == categoryId && (issale ? p.IsSale : true) && (isnew ? p.IsNew : true)).CountAsync();
            }
            else
            {
                return await _context.Products.Where(p => p.IsActive && (issale ? p.IsSale : true) && (isnew ? p.IsNew : true)).CountAsync();
            }
            
        }
        
        public async Task<int> SearchCount(int categoryId,string q)
        {
            if (categoryId != 0)
            {
                return await _context.Products
                    .Where(p => p.IsActive
                    && p.CategoryId == categoryId
                    && (p.NameRu.Contains(q)
                    || p.NameEn.Contains(q)))
                    .CountAsync();
            }
            else
            {
                return await _context.Products
                    .Where(p => p.IsActive
                    && (p.NameRu.Contains(q)
                    || p.NameEn.Contains(q)))
                    .CountAsync();
            }
        }

        public async Task<List<OutProductModel>> Search(int rows_limit, int rows_offset, int categoryId,string q)
        {
            List<Product> products = new List<Product>();
            if (categoryId != 0)
            {
                products = await _context.Products
                    .Where(p => p.IsActive 
                    && p.CategoryId == categoryId 
                    && (p.NameRu.Contains(q)
                    || p.NameEn.Contains(q)))
                    .Skip(rows_offset)
                    .Take(rows_limit)
                    .ToListAsync();
            }
            else
            {
                products = await _context.Products
                    .Where(p => p.IsActive
                    && (p.NameRu.Contains(q)
                    || p.NameEn.Contains(q)))
                    .Skip(rows_offset)
                    .Take(rows_limit)
                    .ToListAsync();
            }
            List<OutProductModel> prods = new List<OutProductModel>();
            foreach (var prod in products)
            {
                prods.Add(_mapper.Map<Product, OutProductModel>(prod));
            }

            return prods;
        }
    }
}