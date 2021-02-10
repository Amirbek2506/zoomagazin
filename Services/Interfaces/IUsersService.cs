using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using ZooMag.Models;
using ZooMag.ViewModels;

namespace ZooMag.Services.Interfaces
{
    public interface IUserService
    {
        UserModel FetchById(int id);
        Task<List<Role>> GetRoles();
        Task<List<Gender>> GetGenders();
        Task<List<UserModel>> FetchWorkers();
        Task<List<UserModel>> FetchСlients();
        Task<Response> UpdateUser(UserModel userModel);
        Task<Response> SetRole(int userId,int roleId);
        Task<Response> DeleteUser(int id);
        void Delete(int id);
        Task<int> Save();
    }
}