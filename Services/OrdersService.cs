using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZooMag.Data;
using ZooMag.DTOs.Order;
using ZooMag.Entities;
using ZooMag.Services.Interfaces;
using ZooMag.ViewModels;

namespace ZooMag.Services
{
    public class OrdersService : IOrdersService
    {
        private readonly ApplicationDbContext _context;


        public OrdersService(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<Response> CreateAsync(CreateOrderRequest request,int userId)
        {
            var cartProductItems = await _context.Baskets.Where(x => x.UserId == userId.ToString()).ToListAsync();
            
            var order = new Order
            {
                Comment = request.Comment,
                Email = request.Email,
                DeliveryTime = request.DeliveryTime.Date,
                OrderDate = DateTime.Now,
                PickupPointId = request.PickupPointId,
                OrderStatusId = 1,
                UserId = userId,
                DeliveryAddress = request.DeliveryAddress,
                IsDelivery = !request.PickupPointId.HasValue,
                DeliveryFrom = request.DeliveryFrom,
                DeliveryUntil = request.DeliveryUntil,
                OrderProductItems = cartProductItems.Select(x=> new OrderProductItem
                {
                    ProductItemId = x.ProductItemId,
                    Count = x.Count
                }).ToList()
            };

            _context.Baskets.RemoveRange(cartProductItems);
            
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
            return new Response {Status = "success", Message = "Успешно"};
        }

        public async Task<Response> UpdateOrderStatusAsync(UpdateOrderStatusRequest request)
        {
            var order = await _context.Orders.FindAsync(request.OrderId);
            if (order == null)
                return new Response
                {
                    Message = "Не найден",
                    Status = "error"
                };
            order.OrderStatusId = request.StatusId;
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
            return new Response
            {
                Message = "Успешно",
                Status = "success"
            };
        }

        public async Task<List<OrderResponse>> GetUserOrders(int userId)
        {
            return await _context.Orders
                .Where(x => x.UserId == userId)
                .Include(x=>x.OrderStatus)
                .Include(x=>x.PickupPoint)
                .Include(x => x.OrderProductItems)
                .ThenInclude(x => x.ProductItem)
                .Select(x => new OrderResponse
                {
                    Id = x.Id,
                    Status = x.OrderStatus.Title,
                    DeliveryAddress = x.PickupPointId.HasValue ? x.PickupPoint.Name : x.DeliveryAddress,
                    OrderDate = x.OrderDate,
                    Summa = x.OrderProductItems.Sum(pi=>Math.Round(pi.ProductItem.Price - pi.ProductItem.Price * pi.ProductItem.Percent / 100, 2))
                }).ToListAsync();
        }
    }
}
