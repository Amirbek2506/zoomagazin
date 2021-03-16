using System.Collections.Generic;
using System.Threading.Tasks;
using ZooMag.Models.Entity;
using ZooMag.Models.ViewModels.Articles;
using ZooMag.ViewModels;

namespace ZooMag.Services.Interfaces
{
    public interface IArticlesService
    {
        Task<Response> Create(InpArticleModel animal);
        Task<Response> Delete(int id);
        Task<List<Article>> Get();
        Task<Article> GetById(int id);
        Task<Response> Update(UpdArticleModel model);
    }
}
