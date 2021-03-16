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
        Task<int> CountClients();
        Task<int> CountWorkers();
        Task<List<Role>> GetRoles();
        Task<List<Gender>> GetGenders();
        Task<List<UserModel>> FetchWorkers(int offset, int limit);
        Task<List<UserModel>> FetchСlients(int offset, int limit);
        Task<Response> UpdateUser(UserModel userModel);
        Task<Response> SetRole(int userId,int roleId);
        Task<Response> DeleteUser(int id);
        Task<int> Save();
    }
}