using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ZooMag.Data;
using ZooMag.DTOs.AdditionalServ;
using ZooMag.Entities;
using ZooMag.Mapping;
using ZooMag.Services.Interfaces;
using ZooMag.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;

namespace ZooMag.Services
{
    public class AdditionalServService : IAdditionalServService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IFileService _fileService; 

        public AdditionalServService(ApplicationDbContext context, IFileService fileservice)
        {
            _context = context;
            _fileService = fileservice;
            _mapper = new MapperConfiguration(x => x.AddProfile<GeneralProfile>()).CreateMapper();
        }
        public async Task<Response> CreateAdditionalService(CreateAdditionalServRequest request)
        {
            var model = _mapper.Map<CreateAdditionalServRequest, AdditionalServ>(request);
            model.CreatedDate = DateTime.Now;

            await _context.AddAsync(model);
            _context.SaveChanges();
            
            if (request.BannerImages != null)
                await this.CreateServImages(request.BannerImages.Select(x => new CreateServImageRequest{ Image = x, IsBannerImage = true}).ToList(), model.Id);
            if(request.OtherImages != null)
                await this.CreateServImages(request.OtherImages.Select(x => new CreateServImageRequest{ Image = x, IsBannerImage = false}).ToList(), model.Id);
            return new Response {Status = "success", Message = "Услуга успешно добавлена"};
        }

        public async Task<List<int>> CreateServImages(List<CreateServImageRequest> request, int addiotionalServId)
        {
            var imagePaths = await  _fileService.AddServImageFilesASync(request.Select(x => x.Image).ToList());
            var models = _mapper.Map<List<CreateServImageRequest>, List<ServImages>>(request);
            var result = new List<int>();
            for(var i = 0; i < models.Count(); i++)
            {
                models[i].IsBannerImage = request[i].IsBannerImage;
                models[i].ImageUrl = imagePaths[i];
                models[i].AdditionalServId = addiotionalServId;
                await _context.ServImages.AddAsync(models[i]);
                _context.SaveChanges();
                result.Add(models[i].Id);
            }
            return result;
        }


        public async Task<Response> DeleteAdditionalServ(int additionalServId)
        {
            var entity = await _context.AdditionalServs.FirstOrDefaultAsync(x => x.Id == additionalServId);
            if (entity == null)
                return new Response {Status = "error", Message = "Услуга не найдена"};
            var images = _context.ServImages.Where(x => x.AdditionalServId == additionalServId);
            
            //remove data:
            await images.ForEachAsync(x => _fileService.Delete(x.ImageUrl));
            _context.ServImages.RemoveRange(images);
            _context.AdditionalServs.Remove(entity);
            _context.SaveChanges();
            return new Response {Status = "success", Message = "Услуга успешно удалена"};
        }

        public async Task<Response> DeleteServImage(int servImageId)
        {
            var entity = await _context.ServImages.FirstOrDefaultAsync(x => x.Id == servImageId);
            if (entity == null)
                return new Response {Status = "error", Message = "Изображение не найдено"};
            _fileService.Delete(entity.ImageUrl);
            _context.ServImages.Remove(entity);    
            _context.SaveChanges();
            return new Response {Status = "success", Message = "Изображение успешно удалено"};            
        }

        public async Task<GetAdditionalServResponse> GetAdditionalServ(int additionalServId)
        {
            var entity = _context.AdditionalServs.FirstOrDefault(x => x.Id == additionalServId);
            if (entity == null) return null;

            var result = _mapper.Map<AdditionalServ, GetAdditionalServResponse>(entity);            
            result.ServImages = await this.GetServImages(result.Id);
            return result;
        }

        public async Task<List<GetAdditionalServResponse>> GetAllAdditionalServ()
        {
            var entity = _context.AdditionalServs.ToList();
            if (entity == null) return null;

            var result = _mapper.Map<List<AdditionalServ>, List<GetAdditionalServResponse>>(entity);            
            foreach(var item in result)
            {
                item.ServImages = await this.GetServImages(item.Id);
            }
            return result;
        }

        public async Task<List<GetServImageResponse>> GetServImages(int addiotionalServId)
        {
            var entites = _context.ServImages.Where(x => x.AdditionalServId == addiotionalServId).ToList();
            var result = _mapper.Map<List<ServImages>, List<GetServImageResponse>>(entites);
            return result;
        }

        public async Task<Response> UpdateAdditionalServ(UpdateAdditionalServRequest request)
        {
            var model = _mapper.Map<UpdateAdditionalServRequest, AdditionalServ>(request);
            _context.AdditionalServs.Update(model);
            _context.SaveChanges();

            if (request.BannerImages != null)
                await this.CreateServImages(request.BannerImages.Select(x => new CreateServImageRequest{ Image = x, IsBannerImage = true}).ToList(), model.Id);
            if(request.OtherImages != null)
                await this.CreateServImages(request.OtherImages.Select(x => new CreateServImageRequest{ Image = x, IsBannerImage = false}).ToList(), model.Id);

            return new Response {Status = "success", Message = "Данные успешно обновлены"};
        }
    }
}