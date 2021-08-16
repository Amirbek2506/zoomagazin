using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using ZooMag.Data;
using ZooMag.DTOs.Pet;
using ZooMag.DTOs.PetImage;
using ZooMag.Entities;
using ZooMag.Mapping;
using ZooMag.Models;
using ZooMag.Models.ViewModels.Pets;
using ZooMag.Models.ViewModels.Products;
using ZooMag.Services.Interfaces;
using ZooMag.ViewModels;
using zoomagazin.DTOs.Pet;

namespace ZooMag.Services
{
    public class PetsService : IPetsService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IFileService _fileService; 

        public PetsService(ApplicationDbContext context, IFileService fileservice)
        {
            _context = context;
            _fileService = fileservice;
            _mapper = new MapperConfiguration(x => x.AddProfile<GeneralProfile>()).CreateMapper();
        }

        public async Task<int> CreatePet(CreatePetRequest request)
        {
            //incorrect PetCategory
            if (_context.PetCategories.FirstOrDefault(x => x.Id == request.PetCategoryId) == null)
                return 0;
            //create Pet entity
            var model = _mapper.Map<CreatePetRequest, Pet>(request);
            await _context.Pets.AddAsync(model);
            _context.SaveChanges();
            //create pets images
            if (request.Images != null)
            {
                var ImagePaths = await this.CreatePetGalery(new CreatePetImagesRequest{
                                        PetId = model.Id,
                                        Images = request.Images
                                    });
                model.MainImageId = ImagePaths.First();
                _context.SaveChanges();
            }
            return model.Id;            
        }

        public async Task<IEnumerable<int>> CreatePetGalery(CreatePetImagesRequest request)
        {
            var imagePaths = await  _fileService.AddPetGalleryFilesASync(request.Images);
            var models = imagePaths.Select(x => new PetImage{PetId = request.PetId, ImageUrl = x});
            var result = new List<int>();
            foreach (var item in models)
            {
                await _context.PetImages.AddAsync(item);
                _context.SaveChanges();
                result.Add(item.Id);
            }              
            return result;
        }

        public async Task<Response> DeletePet(int petId)
        {
            var entity = await _context.Pets.FirstOrDefaultAsync(x => x.Id == petId);
            if (entity == null)
                return new Response {Status = "error", Message = "Питомец не найден"};
            var galeries = _context.PetImages.Where(x => x.PetId == petId);
            
            //remove data:
            _context.PetImages.RemoveRange(galeries);
            _context.Pets.Remove(entity);
            _context.SaveChanges();
            return new Response {Status = "success", Message = "Питомец успешно удалён"};
        }

        public async Task DeletePetGalery(int petGaleryId)
        {
            var entity = await _context.PetImages.FirstOrDefaultAsync(x => x.Id == petGaleryId);
            if (entity == null)
                return;
            _context.PetImages.Remove(entity);
        }

        public async Task<Response> DeletePetImage(int petImageId)
        {
            var entity = await _context.PetImages.FirstOrDefaultAsync(x => x.Id == petImageId);
            if (entity == null)
                return new Response {Status = "error", Message = "Изображение не найдено"};
            _fileService.Delete(entity.ImageUrl);
            _context.PetImages.Remove(entity);    
            _context.SaveChanges();
            return new Response {Status = "success", Message = "Изображение успешно удалено"};            
        }

        public async Task<List<PetListItemResponse>> GetAllPets()
        {
            var entities = await _context.Pets.Where(x => x.IsActive).Include(x => x.PetImages).ToListAsync();
            var result = _mapper.Map<List<Pet>, List<PetListItemResponse>>(entities);            
            return result;
        }

        public async Task<GetPetResponse> GetPet(int id)
        {
            var entity = _context.Pets.FirstOrDefault(x => x.Id == id);
            if (entity == null) return null;

            var result = _mapper.Map<Pet, GetPetResponse>(entity);            
            result.Images = await GetPetGalery(entity.Id, entity.MainImageId);
            if ( _context.PetImages.FirstOrDefault(x => x.Id == entity.MainImageId) != null)
                result.Image = _context.PetImages.FirstOrDefault(x => x.Id == entity.MainImageId).ImageUrl;
            return result;
        }

        public async Task<List<GetPetImageResponse>> GetPetGalery(int petId, int? mainImageId)
        {
            var petGaleriesEntities = _context.PetImages.Where(x => x.PetId == petId && x.Id != (mainImageId??0)).ToList();
            var result = _mapper.Map< List<PetImage>, List<GetPetImageResponse>>(petGaleriesEntities);
            return result;
        }

        public async Task<Response> UpdatePet(UpdatePetRequest request)
        {
            var model = _mapper.Map<UpdatePetRequest, Pet>(request);
            _context.Pets.Update(model);
            _context.SaveChanges();
            await CreatePetGalery(new CreatePetImagesRequest{ PetId = model.Id, Images = request.NewImages.ToList()});
            return new Response {Status = "success", Message = "Данные успешно обновлены"};;
        }
    }
}