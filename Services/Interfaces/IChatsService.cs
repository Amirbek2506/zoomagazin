using System.Collections.Generic;
using System.Threading.Tasks;
using ZooMag.Entities;
using ZooMag.Models.ViewModels.Chats;
using ZooMag.ViewModels;

namespace ZooMag.Services.Interfaces
{
    public interface IChatsService
    {
        Task<List<Animal>> FetchUnreadAnimals(int animalid);
        Task<List<Animal>> FetchAnimals(int animalid);
        Task<int> CountUnreadMessages(int animalid);
        Task<List<Chat>> Get(int fromanimalid, int toanimalid);
        Task<bool> Send(InpChatModel model,int userid);
        Task<Response> DeleteMessage(int id,int userid);
        Task<Response> Delete(int fromanimalid, int toanimalid);
    }
}
