﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ZooMag.Data;
using ZooMag.DTOs.ProductItem;
using ZooMag.Entities;
using ZooMag.Services.Interfaces;
using ZooMag.ViewModels;

namespace ZooMag.Services
{
    public class ProductItemService : IProductItemService
    {
        private readonly ApplicationDbContext _context;
        private readonly IFileService _fileService;

        public ProductItemService(ApplicationDbContext context, IFileService fileService)
        {
            _context = context;
            _fileService = fileService;
        }

        public async Task<Response> UpdateAsync(UpdateProductItemRequest request)
        {
            var productItem = await _context.ProductItems.Where(x=>x.Id == request.Id).Include(x=>x.ProductItemImages).FirstOrDefaultAsync();

            if (productItem == null)
                return new Response { Status = "error", Message = "Не найден" };

            productItem.Measure = request.Measure;
            productItem.Percent = request.Percent;
            productItem.Price = request.Price;
            productItem.VendorCode = request.VendorCode;
            if(productItem.ProductItemImages.Count > 0)
            {
                var productItemImages = await _fileService.AddProductItemFilesASync(request.Images);
                await _context.ProductItemImages.AddRangeAsync(productItemImages.Select(x => new ProductItemImage
                    {ImagePath = x, ProductItemId = request.Id}));
            }

            await _context.SaveChangesAsync();

            return new Response {Status = "success", Message = "Успешно"};
        }

        public async Task<Response> DeleteAsync(int id)
        {
            var productItem = await _context.ProductItems.FindAsync(id);

            if (productItem == null)
                return new Response {Status = "success", Message = "Не найден"};

            var productItemsCount =
                await _context.ProductItems.Where(x => x.ProductId == productItem.ProductId).CountAsync();

            if (productItemsCount == 1)
                return new Response {Status = "error", Message = "Нельзя удалять последний продукт"};
            
            productItem.Removed = true;

            var productItemImages = await _context.ProductItemImages.Where(x => x.ProductItemId == id).ToListAsync();

            foreach (var productItemImage in productItemImages)
            {
                _fileService.Delete(productItemImage.ImagePath);
            } 
            
            _context.ProductItemImages.RemoveRange(productItemImages);

            string emptyFilePath = Path.GetFullPath("Resources/Images/Products/image/png");
            
            await _context.ProductItemImages.AddAsync(new ProductItemImage
                {ImagePath = emptyFilePath, ProductItemId = id});

            return new Response {Status = "success", Message = "Успешно"};
        }

        public async Task<Response> CreateAsync(CreateProductItemRequest request)
        {
            var productItemImages = await _fileService.AddProductItemFilesASync(request.Images);

            if (request.ProductId != null)
            {
                var productItem = new ProductItem
                {
                    Descriptions = request.Descriptions.Select(x => new Description
                    {
                        Content = x.Content,
                        Title = x.Title
                    }).ToList(),
                    Measure = request.Measure,
                    Percent = request.Percent,
                    Price = request.Price,
                    Removed = false,
                    VendorCode = request.VendorCode,
                    ProductId = (int) request.ProductId,
                    ProductItemImages = productItemImages.Count > 0
                        ? productItemImages.Select(x => new ProductItemImage {ImagePath = x}).ToList()
                        : new List<ProductItemImage>
                            {new ProductItemImage {ImagePath = Path.GetFullPath("Resources/Images/Products/image.png")}}
                };
                
                await _context.ProductItems.AddAsync(productItem);
                await _context.SaveChangesAsync();
                
                return new Response {Message = "Успешно", Status = "success"};
            }

            return new Response {Status = "success", Message = "Не найден"};
        }
    }
}