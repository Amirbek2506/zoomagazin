using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ZooMag.Data;
using ZooMag.Entities;
using ZooMag.Mapping;
using ZooMag.Services.Interfaces;
using ZooMag.ViewModels;

namespace ZooMag.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;


        public UserService(ApplicationDbContext context)
        {
            _context = context;
            _mapper = new MapperConfiguration(x => x.AddProfile<GeneralProfile>()).CreateMapper();
        }
        public async Task<int> CountClients()
        {
            return await _context.Users
                 .Join(_context.UserRoles.Where(t => t.RoleId == 3), us => us.Id, ur => ur.UserId, (us, ur) => us)
                 .CountAsync();
        }
        public async Task<int> CountWorkers()
        {
            return await _context.Users
                 .Join(_context.UserRoles.Where(t => t.RoleId != 3), us => us.Id, ur => ur.UserId, (us, ur) => us)
                 .CountAsync();
        }
        public async Task<List<UserModel>> FetchWorkers(int offset, int limit)
        {
            var userList = new List<UserModel>();
            var users = await _context.Users
                 .Join(_context.UserRoles.Where(t => t.RoleId != 3), us => us.Id, ur => ur.UserId, (us, ur) => us)
                 .Skip(offset)
                 .Take(limit)
                 .ToListAsync();
            foreach (var user in users.OrderBy(p => p.Id).ToList())
            {
                var userRolesIds = await _context.UserRoles
                    .Where(m => m.UserId == user.Id && m.RoleId != 3)
                    .Select(p => p.RoleId)
                    .ToListAsync();
                if (userRolesIds.Count() != 0)
                {
                    var model = _mapper.Map<User, UserModel>(user);
                    model.Roles = await _context.Roles.Where(r => userRolesIds.Contains(r.Id)).ToListAsync();
                    userList.Add(model);
                }
            }
            return userList;
        }

        public async Task<List<UserModel>> FetchСlients(int offset, int limit)
        {
            var userList = new List<UserModel>();
            var users = await _context.Users
                .Join(_context.UserRoles.Where(t => t.RoleId == 3), us => us.Id, ur => ur.UserId, (us, ur) => us)
                .Skip(offset)
                .Take(limit)
                .ToListAsync();
            foreach (var user in users)
            {
                var userRole = await _context.UserRoles.FirstOrDefaultAsync(m => m.UserId == user.Id && m.RoleId == 3);
                var model = _mapper.Map<User, UserModel>(user);
                if (userRole != null)
                {
                    model.Roles = await _context.Roles.Where(r => r.Id == userRole.RoleId).ToListAsync();
                }
                userList.Add(model);
            }
            return userList;
        }

        public async Task<Response> SetRole(int userId, int roleId)
        {
            var userrole = await _context.UserRoles.Where(p => p.UserId == userId).ToListAsync();
            _context.RemoveRange(userrole);
            await Save();
            if (await _context.UserRoles.FirstOrDefaultAsync(p => p.RoleId == roleId && p.UserId == userId) == null)
            {
                if (await _context.Roles.FirstOrDefaultAsync(p => p.Id == roleId) == null)
                {
                    return new Response { Status = "error", Message = "Такой рол не существует!" };
                }
                if (await _context.Users.FirstOrDefaultAsync(p => p.Id == userId) == null)
                {
                    return new Response { Status = "error", Message = "Ползователь не существует!" };
                }
                _context.UserRoles.Add(new IdentityUserRole<int> { UserId = userId, RoleId = roleId });
                await Save();
                return new Response { Status = "success", Message = "Рол успешно присвоен!" };
            }
            return new Response { Status = "error", Message = "Рол был присвоен!" };
        }

        public async Task<Response> UpdateUser(UserModel userModel)
        {
            User user = await _context.Users.FirstOrDefaultAsync(p => p.Id == userModel.Id);
            if (user != null)
            {
                if (userModel.file != null)
                {
                    string fileName = await UploadImage(user.Id, userModel.file);
                    user.Image = "Resources/Images/Users/" + user.Id + "/" + fileName;
                }
                user.LastName = userModel.LastName;
                user.FirstName = userModel.FirstName;
                user.GenderId = userModel.GenderId;
                user.BirthDay = userModel.BirthDay;
                await Save();
                return new Response { Status = "success", Message = "Ползователь успешно изменен!" };
            }
            return new Response { Status = "error", Message = "Такого ползователя не существует!" };
        }

        public async Task<string> UploadImage(int userId, IFormFile file)
        {
            string path = Path.GetFullPath("Resources/Images/Users/" + userId);
            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
            }
            Directory.CreateDirectory(path);
            path = Path.Combine(path, file.FileName);
            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            return file.FileName;
        }


        public async Task<List<Role>> GetRoles()
        {
            return await _context.Roles.ToListAsync();
        }

        public async Task<List<Gender>> GetGenders()
        {
            return await _context.Genders.ToListAsync();
        }


        public async Task<Response> DeleteUser(int id)
        {
            try
            {
                User user = _context.Users.Find(id);
                if (user == null)
                    return new Response { Status = "error", Message = "Ползователь не существует!" };
                List<Entities.Wishlist> wishlist = await _context.Wishlists.Where(p => p.UserId == id.ToString()).ToListAsync();
                if (wishlist.Count() > 0)
                {
                    _context.Wishlists.RemoveRange(wishlist);
                }
                List<Cart> carts = await _context.Carts.Where(p => p.UserKey == id.ToString()).ToListAsync();
                if (carts.Count() > 0)
                {
                    _context.Carts.RemoveRange(carts);
                }
                List<Order> orders = await _context.Orders.Where(p => p.UserId == id.ToString()).ToListAsync();
                if (orders.Count() > 0)
                {
                    foreach (var item in orders)
                    {
                        _context.Orders.Remove(item);
                        _context.OrderItems.RemoveRange(await _context.OrderItems.Where(p => p.OrderId == item.Id).ToListAsync());
                    }
                }

                DeleteDirectory(id);
                await DeleteAnimals(id);
                _context.Users.Remove(user);
                await Save();
                return new Response { Status = "success", Message = "Пользователь успешно удален!" };
            }
            catch (Exception ex)
            {
                return new Response { Status = "error", Message = ex.Message };
            }
        }

        private async Task DeleteAnimals(int userid)
        {
            var animals = await _context.Animals.Where(p => p.UserId == userid).ToListAsync();
            foreach (var animal in animals)
            {
                string path = Path.GetFullPath("Resources/Images/Animals/" + animal.Id);
                if (Directory.Exists(path))
                {
                    Directory.Delete(path, true);
                }
                var chats = await _context.Chats.Where(p => p.FromAnimalId == animal.Id || p.ToAnimalId == animal.Id).ToListAsync();
                _context.RemoveRange(chats);
                _context.Animals.Remove(animal);
                await Save();
            }
            return;
        }

        public async Task<int> Save()
        {
            return await _context.SaveChangesAsync();
        }
        private void DeleteDirectory(int id)
        {
            string path = "Resources/Images/Users/" + id;
            if (Directory.Exists(path))
                Directory.Delete(path, true);
        }
    }
}