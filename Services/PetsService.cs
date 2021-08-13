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
using ZooMag.DTOs.PetGalery;
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
            var model = _mapper.Map<CreatePetRequest, Pet>(request);
            await _context.Pets.AddAsync(model);
            _context.SaveChanges();

            //create Pet image and Galery:
            if (request.Image != null)
            {
                await CreatePetMainImage(new CreatePetMainImageRequest{
                    PetId = model.Id,
                    Image = request.Image
                });
            }
            if (request.Images != null)
            {
                await this.CreatePetGalery(new CreatePetGaleryRequest{
                    PetId = model.Id,
                    Images = request.Images
                });
            }
            return model.Id;
            /*
            var productItemImages = new List<string>();
            if(request.Images != null)
                productItemImages = await _fileService.AddProductItemFilesASync(request.Images);
            else 
                productItemImages.Add("Resources/no-image.png");
            if (request.ProductId != null)
            {
                List<CreateDescriptionRequest> descriptionRequests = new List<CreateDescriptionRequest>();
                if(request.Descriptions.Any(x=> !string.IsNullOrEmpty(x)))
                   descriptionRequests = request.Descriptions.Select(JsonConvert.DeserializeObject<CreateDescriptionRequest>).ToList();
                var productItem = new ProductItem
                {
                    Descriptions = descriptionRequests.Select(x => new Description
                    {
                        Content = x.Content,
                        Title = x.Title
                    }).ToList(),
                    Measure = request.Measure,
                    Percent = request.Percent,
                    Price = request.Price,
                    Removed = false,
                    VendorCode = request.VendorCode,
                    ProductId = (int) request.ProductId,
                    ProductItemImages = productItemImages.Select(x => new ProductItemImage {ImagePath = x}).ToList()
                };
                
                await _context.ProductItems.AddAsync(productItem);
                await _context.SaveChangesAsync();
                
                return new Response {Message = "Успешно", Status = "success"};
            }

            return new Response {Status = "success", Message = "Не найден"};
            */
        }

        public async Task<List<int>> CreatePetGalery(CreatePetGaleryRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task<int> CreatePetMainImage(CreatePetMainImageRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task<GetPetResponse> GetPet(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<GetPetGaleryResponse> GetPetGalery(int petId)
        {
            throw new NotImplementedException();
        }



        // public async Task<int> CreatePet(InpPetModel pet)
        // {
        //     Pet prod = _mapper.Map<InpPetModel, Pet>(pet);
        //     if (await _context.PetCategories.FindAsync(prod.PetCategoryId)==null)
        //     {
        //         prod.PetCategoryId = 0;
        //     }
        //     prod.Image = "http://api.zoomag.tj/Resources/Images/Pets/image.png";
        //     prod.IsActive = true;
        //     _context.Pets.Add(prod);
        //     await Save();
        //     return prod.Id;
        // }

        // public OutPetModel FetchPetById(int id)
        // {
        //     Pet prod = _context.Pets.FirstOrDefault(p => p.Id == id && p.IsActive);
        //     if (prod == null)
        //         return null;

        //     var pet = _mapper.Map<Pet, OutPetModel>(prod);
        //     pet.Category = _context.PetCategories.Find(prod.PetCategoryId);

        //     return pet;
        // }

        // public async Task<List<OutPetModel>> FetchPets(int rows_limit, int rows_offset,int categoryId)
        // {
        //     List<Pet> pets = await _context.Pets
        //         .Where(p => 
        //             p.IsActive && 
        //             (categoryId != 0 ? p.PetCategoryId == categoryId : true))
        //         .Skip(rows_offset).Take(rows_limit).ToListAsync();

        //     return _mapper.Map<List<Pet>, List<OutPetModel>>(pets);
        // }

        // public async Task<int> UpdatePet(UpdPetModel pet)
        // {
        //     Pet prod = await _context.Pets.SingleOrDefaultAsync(p => p.Id == pet.Id && p.IsActive);
        //     if (prod == null)
        //         return 0;

        //     prod.PetCategoryId = await _context.PetCategories.FindAsync(prod.PetCategoryId) == null?0:pet.PetCategoryId;

        //     prod.Name = pet.Name;
        //     prod.Discription = pet.Discription;
        //     prod.Color = pet.Color;
        //     prod.Price = pet.Price;
        //     prod.Quantity = pet.Quantity;
        //     prod.Breed = pet.Breed;
        //     prod.Age = pet.Age;

        //     await Save();
        //     return prod.Id;
        // }

        // public async Task<Response> DeletePet(int id)
        // {
        //     Pet pet = _context.Pets.FirstOrDefault(p => p.Id == id);
        //     if (pet != null)
        //     {
        //         DeleteDirectory(id);
        //         await DeletePetGaleries(id);
        //         pet.Image = "Resources/Images/deleted.png";
        //         pet.IsActive = false;
        //         await Save();
        //         return new Response { Status = "success", Message = "Питомец успешно удален!" };
        //     }
        //         return new Response { Status = "error", Message = "Питомец не существует!" };
        // }       


        // #region pet images
        // private async Task<string> UploadImage(int petId, IFormFile file)
        // {
        //     string fName = Guid.NewGuid().ToString() + file.FileName;
        //     string path = Path.GetFullPath("Resources/Images/Pets/" + petId);
        //     if (!Directory.Exists(path))
        //     {
        //         Directory.CreateDirectory(path);
        //     }
        //     path = Path.Combine(path, fName);
        //     using (var stream = new FileStream(path, FileMode.Create))
        //     {
        //         await file.CopyToAsync(stream);
        //     }
        //     return "http://api.zoomag.tj/Resources/Images/Pets/" + petId + "/" + fName;
        // }


        // public async Task CreatePetGaleries(int petId, IFormFile[] images)
        // {
        //     for (int i = 1; i <= images.Length; i++)
        //     {
        //         string fileName = await UploadImage(petId, images[i - 1]);
        //         if (i == 1)
        //         {
        //             Pet pet = _context.Pets.FirstOrDefault(p => p.Id == petId);
        //             if (pet != null)
        //             {
        //                 if(pet.Image == "http://api.zoomag.tj/Resources/Images/Pets/image.png" || String.IsNullOrEmpty(pet.Image))
        //                 {
        //                     pet.Image = fileName;
        //                     await Save();
        //                     continue;
        //                 }
        //             }
        //         }
        //         _context.PetGaleries.Add(
        //             new PetGalery
        //             {
        //                 PetId = petId,
        //                 Image = fileName
        //             });
        //     }
        //     await Save();
        //     return;
        // }

        // public async Task<Response> SetMainImage(int petId,int imageId)
        // {
        //     var pet = await _context.Pets.FindAsync(petId);
        //     if(pet==null)
        //     {
        //         return new Response { Status = "error",Message = "Питомец   не найден!" };
        //     }
        //     var galery = await _context.PetGaleries.FindAsync(imageId);
        //     if (galery == null)
        //     {
        //         return new Response { Status = "error", Message = "Фото не найдено!" };
        //     }
        //     string img = pet.Image;
        //     pet.Image = galery.Image;
        //     galery.Image = img;
        //     await Save();
        //     return new Response {Status = "success",Message = "Фото успешно присвоен!" }; ;
        // }

        // private async Task DeletePetGaleries(int petId)
        // {
        //     var galeries = await _context.PetGaleries.Where(p => p.PetId == petId).ToListAsync();
        //         _context.PetGaleries.RemoveRange(galeries);
        //     await Save();
        //     return;
        // }

        // public async Task<List<PetImagesModel>> FetchPetGaleriesByPetId(int petId)
        // {
        //     var res = await _context.PetGaleries.Where(p => p.PetId == petId).ToListAsync();
        //     return _mapper.Map<List<PetGalery>, List<PetImagesModel>>(res);
        // }

        // private void DeleteDirectory(int petId)
        // {
        //     string path = "Resources/Images/Pets/" + petId;
        //     if (Directory.Exists(path))
        //         Directory.Delete(path, true);
        // }

        // private void DeleteImage(string path)
        // {
        //     FileInfo fileInfo = new FileInfo(path);
        //     if (fileInfo.Exists)
        //     {
        //         fileInfo.Delete();
        //     }
        // }


        // public async Task<Response> DeleteImage(int id, int petId)
        // {
        //     if (id != 0)
        //     {
        //         var galery = _context.PetGaleries.FirstOrDefault(p => p.Id == id);
        //         if(galery!=null)
        //         {
        //             DeleteImage(galery.Image);
        //             await DeletePetGalery(id);
        //             return new Response { Status = "success", Message = "Фотография успешно удалено!" };
        //         }
        //     }
        //     else
        //     {
        //         var pet = await _context.Pets.FirstOrDefaultAsync(p=>p.Id == petId);
        //         if (pet!=null)
        //         {
        //             DeleteImage(pet.Image);
        //             var galery = _context.PetGaleries.FirstOrDefault(p => p.PetId == petId);
        //             if (galery != null)
        //             {
        //                 pet.Image = galery.Image;
        //                 await Save();
        //                 await DeletePetGalery(galery.Id);
        //             }else
        //             {
        //                 pet.Image = null;
        //                 await Save();
        //             }
        //             return new Response { Status = "success", Message = "Фотография успешно удалено!" };
        //         }
        //     }
        //     return new Response { Status = "error", Message = "Фотография не найдена!" };
        // }


        // private async Task<int> DeletePetGalery(int id)
        // {
        //     var galery = await _context.PetGaleries.FirstOrDefaultAsync(p=>p.Id == id);
        //     if(galery!=null)
        //     {
        //         _context.PetGaleries.Remove(galery);
        //     }
        //     return await _context.SaveChangesAsync();
        // }

        // #endregion

        // private async Task<int> Save()
        // {
        //     return await _context.SaveChangesAsync();
        // }


        // public async Task<int> CountPets(int categoryId)
        // {
        //     return await _context.Pets
        //          .Where(p =>
        //                p.IsActive &&
        //                (categoryId != 0 ? p.PetCategoryId == categoryId : true))
        //          .CountAsync();

        // }
    }
}