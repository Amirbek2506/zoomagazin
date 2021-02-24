using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZooMag.Controllers;
using ZooMag.Data;
using ZooMag.Mapping;
using ZooMag.Models;
using ZooMag.Models.ViewModels.Carts;
using ZooMag.Models.ViewModels.Orders;
using ZooMag.Models.ViewModels.Products;
using ZooMag.Services.Interfaces;
using ZooMag.ViewModels;

namespace ZooMag.Services
{
    public class OrdersService : IOrdersService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;


        public OrdersService(ApplicationDbContext context, ICartsService cartsService)
        {
            _context = context;
            _mapper = new MapperConfiguration(x => x.AddProfile<GeneralProfile>()).CreateMapper();
        }

        public async Task<int> Count()
        {
            return await _context.Orders.CountAsync();
        }

        public async Task<Response> Create(InpOrderModel orderModel, string userKey)
        {
            try
            {
                var carts = await _context.Carts.Where(p => p.UserKey == userKey).ToListAsync();
                if (carts.Count() == 0)
                {
                    return new Response { Status = "error", Message = "Корзина пуста!" };
                }
                if (orderModel.PhoneNumber.Length != 9)
                {
                    return new Response { Status = "error", Message = "Неверный номер телефон!" };
                }
                var order = new Order
                {
                    UserKey = userKey,
                    PhoneNumber = orderModel.PhoneNumber,
                    DeliveryType = orderModel.DeliveryType == 1 ? "Доставка" : "Самовывоз",
                    DeliveryAddress = orderModel.DeliveryAddress,
                    PaymentMethodId = orderModel.PayMethodId,
                    OrderStatusId = 1,
                    OrderSumm = carts.Sum(p => p.Price),
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };
                _context.Orders.Add(order);
                await Save();
                foreach (var cart in carts)
                {
                    _context.OrderItems.Add(
                        new OrderItem
                        {
                            OrderId = order.Id,
                            ProductId = cart.ProductId,
                            Price = cart.Price,
                            SizeId = cart.SizeId,
                            Quantity = cart.Quantity
                        });
                }
                _context.Carts.RemoveRange(carts);
                await Save();
                return new Response { Status = "success", Message = "Заказ успешно оформлен!" };
            }
            catch (Exception ex)
            {
                return new Response { Status = "error", Message = ex.Message };
            }

        }
        public async Task<List<Order>> FetchMyOrders(string userKey)
        {
            var orders = await _context.Orders.Where(p => p.UserKey == userKey).ToListAsync();
            foreach (var order in orders)
            {
                order.OrderStatus = await _context.OrderStatuses.FindAsync(order.OrderStatusId);
                order.PaymentMethod = await _context.PaymentMethods.FindAsync(order.PaymentMethodId);
                //order.OrderItems = await _context.OrderItems.Where(p => p.OrderId == order.Id).ToListAsync();
            }
            return orders;
        }

        public async Task<List<Order>> FetchAll(int offset, int limit)
        {
            var orders = await _context.Orders.Skip(offset).Take(limit).ToListAsync();
            foreach (var order in orders)
            {
                order.OrderStatus = await _context.OrderStatuses.FindAsync(order.OrderStatusId);
                order.PaymentMethod = await _context.PaymentMethods.FindAsync(order.PaymentMethodId);
               // order.OrderItems = await _context.OrderItems.Where(p => p.OrderId == order.Id).ToListAsync();
            }

            return orders;
        }

        public async Task<Response> SetSize(int orderitemid, int sizeid)
        {
            var orderitem = await _context.OrderItems.FindAsync(orderitemid);
            if (orderitem == null)
            {
                return new Response { Status = "error", Message = "Не найден!" };
            }
            var productsize = await _context.ProductSizes
                .FirstOrDefaultAsync
                (
                p => p.ProductId == orderitem.ProductId &&
                p.SizeId == sizeid
                );
            if (productsize == null)
            {
                return new Response { Status = "error", Message = "Размер не найден!" };
            }

            orderitem.SizeId = sizeid;
            await Save();
            return new Response { Status = "success", Message = "Размер успешно присвоен!" };
        }

        private async Task<int> Save()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task<Response> Delete(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if(order==null)
            {
                return new Response { Status = "error", Message = "Не найден!" };
            }
            _context.OrderItems.RemoveRange(
                await _context.OrderItems.Where(
                    p=>p.OrderId == order.Id).ToListAsync());

            _context.Orders.Remove(order);
            await Save();
            return new Response { Status = "success", Message = "Успешно удален!" };
        }



        public async Task<Response> DeleteItem(int id)
        {
            var orderitem = await _context.OrderItems.FindAsync(id);
            if (orderitem == null)
            {
                return new Response { Status = "error", Message = "Не найден!" };
            }
            int count = await _context.OrderItems.CountAsync(p => p.ProductId == orderitem.ProductId);
            if (count > 1)
            {
                _context.OrderItems.Remove(orderitem);
                await Save();
            }
            return new Response {Status = "success",Message = "Успешно удален!" };
        }

        public async Task<decimal> IncrQty(int itemid)
        {
            var orderitem = await _context.OrderItems.FindAsync(itemid);
            if (orderitem == null)
            {
                return 0;
            }
            var productPrice = (await _context.Products.FindAsync(orderitem.ProductId)).SellingPrice;

            orderitem.Quantity++;
            orderitem.Price = productPrice * orderitem.Quantity;
            await Save();
            return orderitem.Price;
        }

        public async Task<decimal> DecrQty(int itemid)
        {
            var orderitem = await _context.OrderItems.FindAsync(itemid);
            if (orderitem == null)
            {
                return 0;
            }
            if (orderitem.Quantity > 1)
            {
                var productPrice = (await _context.Products.FindAsync(orderitem.ProductId)).SellingPrice;
                orderitem.Quantity--;
                orderitem.Price = productPrice * orderitem.Quantity;
                await Save();
            }
            return orderitem.Price;
        }

        public async Task<OutOrderModel> FetchDetail(int orderid)
        {
            var order = await _context.Orders.FindAsync(orderid);
            OutOrderModel orderModel = new OutOrderModel();
            if (order!=null)
            {
                orderModel = _mapper.Map<Order, OutOrderModel>(order);
                orderModel.OrderStatus = await _context.OrderStatuses.FindAsync(order.OrderStatusId);
                orderModel.OrderStatus.Orders = null;
                orderModel.PaymentMethod = await _context.PaymentMethods.FindAsync(order.PaymentMethodId);
                orderModel.PaymentMethod.Orders = null;
                orderModel.OrderItems = new List<OrderItemModel>();
                foreach (var item in await _context.OrderItems.Where(p => p.OrderId == orderid).ToListAsync())
                {
                    Product product = await _context.Products.FindAsync(item.ProductId);
                    OutProductModel productModel = new OutProductModel();
                    if (product!=null)
                    {
                        productModel = _mapper.Map<Product, OutProductModel>(product);
                        productModel.Images = _mapper.Map<List<ProductGalery>, List<ProductImagesModel>>(
                            await _context.ProductGaleries.Where(p => p.ProductId == productModel.Id)
                            .ToListAsync());
                        productModel.Sizes = await FetchSizesByProductId(productModel.Id);
                    }
                    

                    orderModel.OrderItems.Add(new OrderItemModel
                    {
                        Id = item.Id,
                        Price = item.Price,
                        Quantity = item.Quantity,
                        Product = productModel,
                        Size = await _context.Sizes.FindAsync(item.SizeId)
                    });

                }
                return orderModel;
            }
            return null;
        }

        private async Task<List<SizeModel>> FetchSizesByProductId(int productId)
        {
            List<Size> sizes = new List<Size>();
            List<ProductSize> ProductSize = await _context.ProductSizes.Where(p => p.ProductId == productId).ToListAsync();
            foreach (var item in ProductSize)
            {
                Size size = await _context.Sizes.Where(s => s.Id == item.SizeId).FirstOrDefaultAsync();
                if (size != null)
                    sizes.Add(size);
            }
            return _mapper.Map<List<Size>, List<SizeModel>>(sizes); ;
        }

    }
}
