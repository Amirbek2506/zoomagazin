using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZooMag.Services.Interfaces
{
    public interface IFileService
    {
        Task<string> AddBrandFileAsync(IFormFile image);
        void Delete(string imagePath);
        Task<List<string>> AddProductItemFilesASync(List<IFormFile> files);
        Task<string> AddProductItemFileASync(IFormFile file);
        Task<string> AddPromotionFileAsync(IFormFile requestImage);
        Task<string> AddBannerFileAsync(IFormFile requestImage);

        //for pets
        Task<List<string>> AddPetGalleryFilesASync (List<IFormFile> files);
        Task<string> AddPetImageFileASync(IFormFile file);
    }
}
