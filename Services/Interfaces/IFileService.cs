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
    }
}
