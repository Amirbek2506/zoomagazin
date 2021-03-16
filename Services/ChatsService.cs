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
using ZooMag.Models.ViewModels.Animals;
using ZooMag.Models.ViewModels.Carts;
using ZooMag.Models.ViewModels.Chats;
using ZooMag.Models.ViewModels.Products;
using ZooMag.Services.Interfaces;
using ZooMag.ViewModels;

namespace ZooMag.Services
{
    public class ChatsService : IChatsService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;


        public ChatsService(ApplicationDbContext context)
        {
            _context = context;
            _mapper = new MapperConfiguration(x => x.AddProfile<GeneralProfile>()).CreateMapper();
        }
        public async Task<List<Chat>> Get(int fromanimalid, int toanimalid)
        {
            var chats = await _context.Chats.Where(p => (p.FromAnimalId == fromanimalid && p.ToAnimalId == toanimalid) || (p.FromAnimalId == toanimalid && p.ToAnimalId == fromanimalid)).ToListAsync();
            foreach(Chat chat in chats)
            {
                if(!chat.IsReaded && chat.ToAnimalId == fromanimalid)
                {
                    chat.IsReaded = true;
                    await Save();
                }
            }
            return chats;
        }

        public async Task<bool> Send(InpChatModel model,int userid)
        {
            var anml = await _context.Animals.FindAsync(model.ToAnimalId);
            var animal = await _context.Animals.FirstOrDefaultAsync(p=>p.Id == model.FromAnimalId && p.UserId == userid);
            if (animal==null || anml==null)
            {
                return false;
            }
            _context.Chats.Add(new Chat 
            {
                Content = model.Content,
                FromAnimalId = model.FromAnimalId,
                ToAnimalId = model.ToAnimalId,
                IsReaded = false,
                CreatedAt = DateTime.Now
            });
            await Save();
            return true;
        }

        private async Task<int> Save()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task<Response> Delete(int fromanimalid, int toanimalid)
        {
            var chats = await _context.Chats.Where(p => (p.FromAnimalId == fromanimalid && p.ToAnimalId == toanimalid) || (p.FromAnimalId == toanimalid && p.ToAnimalId == fromanimalid)).ToListAsync();
            if(chats.Count()==0)
            {
                return new Response { Status = "success", Message = "Успешно удален!" };
            }
            _context.Chats.RemoveRange(chats);
            await Save();
            return new Response { Status = "success", Message = "Успешно удален!" };
        }

        public async Task<Response> DeleteMessage(int id, int userid)
        {
            var chat = await _context.Chats.FindAsync(id);
            if(chat==null)
            {
                return new Response { Status = "error", Message = "Не найден!" };
            }
            var animal = await _context.Animals.FirstOrDefaultAsync(p=>p.Id == chat.FromAnimalId && p.UserId == userid);
            if (animal == null)
            {
                return new Response { Status = "error", Message = "Вы не можете удалить!" };
            }
            _context.Chats.Remove(chat);
            await Save();
            return new Response {Status = "success", Message = "Успешно удален!" };
        }
    }
}
