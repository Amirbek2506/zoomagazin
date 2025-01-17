﻿using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using ZooMag.Services.Interfaces;

namespace ZooMag.Services
{
    public class FileService : IFileService
    {

        private string GetDirectory(string model)
        {
            string baseDirectory = Path.Combine(Directory.GetCurrentDirectory(),$"Resources/{model}/");
            if(!Directory.Exists(baseDirectory))
            {
                Directory.CreateDirectory(baseDirectory);
            }
            return baseDirectory;
        }

        public async Task<string> AddBrandFileAsync(IFormFile image)
        {
            string dirPath = GetDirectory("Brand");
            string imageName = $"{DateTime.Now:dd-MM-yyyy-H-m}_{image.FileName}";
            string imagePath = dirPath + imageName;
            await CopyFileAsync(imagePath, image);
            return $"Resources/Brand/{imageName}";
        }

        private async Task CopyFileAsync(string imagePath, IFormFile file)
        {
            await using var fs = new FileStream(imagePath, FileMode.Create);
            await file.CopyToAsync(fs);
        }

        public void Delete(string imagePath)
        {
            string noImageFilePath = Path.Combine(Directory.GetCurrentDirectory(),"Resources/no-image.png");
            if(File.Exists(imagePath) && imagePath != noImageFilePath)
            {
                File.Delete(imagePath);
            }
        }

        public async Task<List<string>> AddProductItemFilesASync(List<IFormFile> files)
        {
            List<string> imagePaths = new List<string>();
            foreach(var file in files)
            {
                imagePaths.Add(await AddProductItemFileASync(file));
            }
            return imagePaths;
        }

        public async Task<string> AddProductItemFileASync(IFormFile file)
        {
            string dirPath = GetDirectory("Product");
            string imageName = $"{DateTime.Now:dd-MM-yyyy-H-m}_{file.FileName}";
            string imagePath = dirPath + imageName;
            await CopyFileAsync(imagePath, file);
            return $"Resources/Product/{imageName}";
        }

        public async Task<string> AddPromotionFileAsync(IFormFile file)
        {
            string dirPath = GetDirectory("Promotion");
            string imageName = $"{DateTime.Now:dd-MM-yyyy-H-m}_{file.FileName}";
            string imagePath = dirPath + imageName;
            await CopyFileAsync(imagePath, file);
            return $"Resources/Promotion/{imageName}";
        }

        public async Task<string> AddBannerFileAsync(IFormFile file)
        {
            string dirPath = GetDirectory("Banner");
            string imageName = $"{DateTime.Now:dd-MM-yyyy-H-m}_{file.FileName}";
            string imagePath = dirPath + imageName;
            await CopyFileAsync(imagePath, file);
            return $"Resources/Banner/{imageName}";
        }

        public async Task<List<string>> AddPetGalleryFilesASync(List<IFormFile> files)
        {
            List<string> imagePaths = new List<string>();
            foreach(var file in files)
            {
                imagePaths.Add(await AddPetImageFileASync(file));
            }
            return imagePaths;
        }

        public async Task<string> AddPetImageFileASync(IFormFile file)
        {
            string dirPath = GetDirectory("Pet");
            string imageName = $"{DateTime.Now:dd-MM-yyyy-H-m}_{file.FileName}";
            string imagePath = dirPath + imageName;
            await CopyFileAsync(imagePath, file);
            return $"Resources/Pet/{imageName}";
        }

        public async Task<string> AddServImageFileASync(IFormFile file)
        {
            string dirPath = GetDirectory("AdditionalServ");
            string imageName = $"{DateTime.Now:dd-MM-yyyy-H-m}_{file.FileName}";
            string imagePath = dirPath + imageName;
            await CopyFileAsync(imagePath, file);
            return $"Resources/AdditionalServ/{imageName}";
        }

        public async Task<List<string>> AddServImageFilesASync(List<IFormFile> files)
        {
            List<string> imagePaths = new List<string>();
            foreach(var file in files)
            {
                imagePaths.Add(await AddServImageFileASync(file));
            }
            return imagePaths;
        }
    }
}
