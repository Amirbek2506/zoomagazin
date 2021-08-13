using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using ZooMag.DTOs.PetGalery;
using ZooMag.Models;
using ZooMag.Models.ViewModels.Pets;
using ZooMag.Models.ViewModels.Products;
using ZooMag.ViewModels;
using ZooMag.DTOs.Pet;
using zoomagazin.DTOs.Pet;

namespace ZooMag.Services.Interfaces
{
    public interface IPetsService
    {
        Task<int> CreatePet(CreatePetRequest request);
        Task<int> CreatePetMainImage(CreatePetMainImageRequest request);
        Task<List<int>> CreatePetGalery(CreatePetGaleryRequest request);
        Task<GetPetResponse> GetPet(int id);
        Task<GetPetGaleryResponse> GetPetGalery(int petId);


        // Task<int> CreatePet(InpPetModel pet);
        // Task<int> UpdatePet(UpdPetModel pet);
        // Task<int> CountPets(int categoryId);
        // Task<Response> DeletePet(int id);
        // Task<Response> DeleteImage(int id, int petId);
        // OutPetModel FetchPetById(int id);
        // Task<Response> SetMainImage(int petId,int imageId);
        // Task<List<OutPetModel>> FetchPets(int rows_limit, int rows_offset, int categoryId);
        // Task CreatePetGaleries(int petId,IFormFile[] images);
        // Task<List<PetImagesModel>> FetchPetGaleriesByPetId(int petId);
    }
}