using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ZooMag.Data;
using ZooMag.Mapping;
using ZooMag.Models;
using ZooMag.Models.Entity;
using ZooMag.Models.ViewModels.Hostel;
using ZooMag.Services.Interfaces;
using ZooMag.ViewModels;

namespace ZooMag.Services
{
    public class HostelService : IHostelService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public HostelService(ApplicationDbContext context)
        {
            _context = context;
            _mapper = new MapperConfiguration(x => x.AddProfile<GeneralProfile>()).CreateMapper();
        }

        #region Box 

        public async Task<Response> CreateBox(BoxModel model)
        {
            if(String.IsNullOrEmpty(model.Status) || await _context.BoxTypes.FindAsync(model.BoxTypeId) == null)
            {
                return new Response {Status = "error" ,Message = "Invalid model" };
            }
            var box = new Box
            {
                Status = model.Status,
                Number = model.Number,
                BoxTypeId = model.BoxTypeId
            };
            _context.Boxes.Add(box);
            await Save();
            return new Response {Status = "success",Message = "Успешно добавлен!"};
        }

        public async Task<List<Box>> GetBoxes()
        {
            return await _context.Boxes.ToListAsync();
        }

        public async Task<List<Box>> GetFreeBoxes()
        {
            return await _context.Boxes.Where(p => p.Status == "свободно").ToListAsync();
        }

        public async Task<Response> ChangeStatusBox(int id, string status)
        {
            var box = await _context.Boxes.FindAsync(id);
            if(box==null)
            {
                return new Response {Status = "error",Message = "Не найден!" };
            }
            if(String.IsNullOrEmpty(status))
            {
                return new Response { Status = "error", Message = "Статус должен быть не пустим!" };
            }
            box.Status = status;
            await Save();
            return new Response {Status = "success",Message = "Статус успешно присвоен!" };
        }

        public async Task<Response> DeleteBox(int id)
        {
            var box = await _context.Boxes.FindAsync(id);
            if (box == null)
            {
                return new Response { Status = "error", Message = "Не найден!" };
            }
            _context.Boxes.Remove(box);
            await Save();
            return new Response {Status = "success",Message = "Успешно удален!"};
        }

        #endregion

        #region BoxOrder 

        public async Task<Response> CreateOrder(BoxOrderModel model, int userid)
        {
            if (await _context.BoxTypes.FindAsync(model.BoxTypeId) == null)
            {
                return new Response { Status = "error", Message = "BoxType не наден!" };
            }
            if(model.PhoneNumber.ToString().Length != 9)
            {
                return new Response { Status = "error", Message = "Неверный номер телефон!" };
            }
            var boxOrder = new BoxOrder
            {
                BoxTypeId = model.BoxTypeId,
                Comment = model.Comment,
                PhoneNumber = model.PhoneNumber,
                FromDate = model.FromDate,
                ToDate = model.ToDate,
                Status = "Новый заказ",
                BoxId = 0,
                UserId = userid,
                CreatedAt = DateTime.Now
            };
            _context.BoxOrders.Add(boxOrder);
            await Save();
            return new Response { Status = "success", Message = "Успешно добавлен!" };
        }

        public async Task<List<OutBoxOrderModel>> GetBoxOrders()
        {
            var boxOrders = await _context.BoxOrders.ToListAsync();
            var orders = new List<OutBoxOrderModel>();

            foreach(var boxOrder in boxOrders)
            {
                var order = _mapper.Map<BoxOrder, OutBoxOrderModel>(boxOrder);
                order.Box = await _context.Boxes.FindAsync(boxOrder.BoxId);
                order.BoxType = await _context.BoxTypes.FindAsync(boxOrder.BoxTypeId);
                order.User = _mapper.Map<User, UserModel>(await _context.Users.FindAsync(boxOrder.UserId));
                if(order.Box!=null)
                {
                    order.Box.BoxType = null;
                }
                order.BoxType.BoxOrders = null;

                orders.Add(order);
            }
            return orders;
        }

        public async Task<Response> ChangeStatusBoxOrder(int id, string status)
        {
            var boxOrder = await _context.BoxOrders.FindAsync(id);
            if (boxOrder == null)
            {
                return new Response { Status = "error", Message = "Не найден!" };
            }
            if (String.IsNullOrEmpty(status))
            {
                return new Response { Status = "error", Message = "Статус должен быть не пустим!" };
            }
            boxOrder.Status = status;
            await Save();
            return new Response { Status = "success", Message = "Статус успешно присвоен!" };
        }

        public async Task<Response> SetBoxToOrder(int orderid, int boxid)
        {
            var order = await _context.BoxOrders.FindAsync(orderid);
            if(order==null)
            {
                return new Response { Status = "error", Message = "Заказ не найден!" };
            }
            var box = await _context.Boxes.Where(p => p.Status == "свободно").FirstOrDefaultAsync(p=>p.Id == boxid);
            if (box == null)
            {
                return new Response { Status = "error", Message = "Коробка не найдено!" };
            }
            order.BoxId = boxid;
            box.Status = "бронирование";
            await Save();
            return new Response { Status = "success", Message = "Коробка успешно присвоен!" };
        }

        public async Task<Response> DeleteBoxOrder(int id)
        {
            var boxorder = await _context.BoxOrders.FindAsync(id);
            if (boxorder == null)
            {
                return new Response { Status = "error", Message = "Не найден!" };
            }
            _context.BoxOrders.Remove(boxorder);
            await Save();
            return new Response { Status = "success", Message = "Успешно удален!" };
        }

        #endregion

        #region BoxType 

        public async Task<Response> CreateBoxType(BoxTypeModel model)
        {
            if(String.IsNullOrEmpty(model.TitleEn) || String.IsNullOrEmpty(model.TitleRu) || model.Price == 0)
            {
                return new Response { Status = "error", Message = "Invalid model" };
            }

            var boxType = new BoxType
            {
                TitleEn = model.TitleEn,
                TitleRu = model.TitleRu,
                Price = model.Price
            };
            _context.BoxTypes.Add(boxType);
            await Save();
            return new Response { Status = "success", Message = "Успешно добавлен!" };
        }

        public async Task<List<BoxType>> GetBoxTypes()
        {
            return await _context.BoxTypes.ToListAsync();
        }

        public async Task<Response> DeleteBoxType(int id)
        {
            var boxType = await _context.BoxTypes.FindAsync(id);
            if (boxType == null)
            {
                return new Response { Status = "error", Message = "Не найден!" };
            }
            _context.BoxTypes.Remove(boxType);
            await Save();
            return new Response { Status = "success", Message = "Успешно удален!" };
        }

        #endregion


        private async Task<int> Save()
        {
            return await _context.SaveChangesAsync();
        }

    }
}
