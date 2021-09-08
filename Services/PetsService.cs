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

        public async Task<Response> CreatePet(CreatePetRequest request)
        {
            //incorrect PetCategory
            if (_context.PetCategories.FirstOrDefault(x => x.Id == request.PetCategoryId) == null)
                new Response { Status = "Error", Message = "Категория не найдена!" };
            //create Pet entity
            var model = _mapper.Map<CreatePetRequest, Pet>(request);
            model.CreatedAt = DateTime.Now;

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
            return new Response { Status = "Success", Message = "Питомец успешно добавлен!" };            
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

        public async Task<Response> CreatePetImage(CreatePetImageRequest request)
        {
            var imagePath = await _fileService.AddPetImageFileASync(request.Image);
            var model = new PetImage{
                PetId = request.PetId,
                ImageUrl = imagePath
            };
            await _context.PetImages.AddAsync(model);
            _context.SaveChanges();
            return new Response { Status = "Success", Message = "Изображение успешно добавлено!" };     
        }

        public async Task<Response> DeletePet(int petId)
        {
            var entity = await _context.Pets.FirstOrDefaultAsync(x => x.Id == petId);
            if (entity == null)
                return new Response {Status = "error", Message = "Питомец не найден"};
            var galeries = _context.PetImages.Where(x => x.PetId == petId);
            
            //remove data:
            await galeries.ForEachAsync(x => _fileService.Delete(x.ImageUrl));
            _context.PetImages.RemoveRange(galeries);
            _context.Pets.Remove(entity);
            _context.SaveChanges();
            return new Response {Status = "success", Message = "Питомец успешно удалён"};
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
            var entities = await _context.Pets.Include(x => x.PetImages).ToListAsync();
            if (entities == null) return null;
            
            var result = _mapper.Map<List<Pet>, List<PetListItemResponse>>(entities);            
            foreach (var item in result)
            {
                var mainImage = _context.PetImages.FirstOrDefault(x => x.Id == (item.MainImageId ?? 0));
                item.Images = await GetPetGalery(item.Id, item.MainImageId);
                if (mainImage != null)
                {
                    item.Images.Add( _mapper.Map<PetImage, GetPetImageResponse>(mainImage));
                    item.Images.Reverse();
                }

            }
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

        public async Task<Response> SetMainImage(int petId, int petImageId)
        {
            var pet = await _context.Pets.FirstOrDefaultAsync(x => x.Id == petId);
            var petImage = await _context.PetImages.FirstOrDefaultAsync(x => x.Id == petImageId);
            if (pet == null)
                return new Response{ Status = "error", Message = "Не удалось изменить главное изображение, питомец не найден"};
            if (petImage == null)
                return new Response{ Status = "error", Message = "Не удалось изменить главное изображение, изоюражение не найдено"};
            
            pet.MainImageId = petImageId;
            _context.Pets.Update(pet);
            _context.SaveChanges();
            return new Response{ Status = "success", Message = "Главное изображение успешно изменено"};
        }

        public async Task<Response> UpdatePet(UpdatePetRequest request)
        {
            var pet = await _context.Pets.FindAsync(request.Id);
            if (pet == null)
                return new Response { Status = "error", Message = "Питомец не найден" };

            pet.Name = request.Name;
            pet.Description = request.Description;
            pet.Age = request.Age;
            pet.Breed = request.Breed;
            pet.Color = request.Color;
            pet.Price = request.Price;
            pet.OriginCountry = request.OriginCountry;
            pet.PetCategoryId = request.PetCategoryId;
            pet.IsActive = request.IsActive;
            pet.QuantityInStock = request.QuantityInStock;
            _context.Pets.Update(pet); 
            await _context.SaveChangesAsync();
            return new Response {Status = "success", Message = "Данные успешно обновлены"};
        }
    }
}