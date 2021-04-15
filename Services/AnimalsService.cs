using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ZooMag.Data;
using ZooMag.Mapping;
using ZooMag.Models;
using ZooMag.Models.Entity;
using ZooMag.Models.ViewModels.Animals;
using ZooMag.Models.ViewModels.Carts;
using ZooMag.Models.ViewModels.Products;
using ZooMag.Services.Interfaces;
using ZooMag.ViewModels;

namespace ZooMag.Services
{
    public class AnimalsService : IAnimalsService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;


        public AnimalsService(ApplicationDbContext context)
        {
            _context = context;
            _mapper = new MapperConfiguration(x => x.AddProfile<GeneralProfile>()).CreateMapper();
        }

        public async Task<Response> Create(InpAnimalModel animal, int userid)
        {
            try
            {
                Animal anml = new Animal
                {
                    Name = animal.Name,
                    UserId = userid,
                    Age = animal.Age,
                    AnimalGenderId = animal.AnimalGenderId,
                    AnimalTypeId = animal.AnimalTypeId,
                    CreateAt = DateTime.Now,
                    Breed = animal.Breed,
                    Color = animal.Color,
                    Image = "Resources/Images/Animals/default.jpg"
                };
                _context.Animals.Add(anml);
                if (await Save() > 0 && animal.Image != null)
                {
                    anml.Image = await UploadImage(anml.Id, animal.Image);
                    await Save();
                }
                return new Response
                {
                    Status = "success",
                    Message = "Успешно добавлен!"
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    Status = "error",
                    Message = ex.Message
                };
            }
        }

        public async Task<string> UploadImage(int animalId, IFormFile file)
        {
            string path = Path.GetFullPath("Resources/Images/Animals/" + animalId);
            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
            }
            Directory.CreateDirectory(path);
            path = Path.Combine(path, file.FileName);
            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            return "Resources/Images/Animals/" + animalId + "/" + file.FileName;
        }


        public async Task<List<AnimalGender>> GetAnimalGenders()
        {
            return await _context.AnimalGenders.ToListAsync();
        }

        public async Task<List<Animal>> GetAnimals(int typeid,int userid)
        {
            List<Animal> animals = new List<Animal>();
            if (typeid == 0)
            {
                animals = await _context.Animals.Where(p=>p.UserId != userid).ToListAsync();
            }
            else
            {
                animals = await _context.Animals.Where(p => p.AnimalTypeId == typeid && p.UserId != userid).ToListAsync();
            }
            foreach (var animal in animals)
            {
                animal.AnimalGender = null;
                animal.AnimalType = null;
            }
            return animals;
        }

        public async Task<Animal> GetAnimalById(int id)
        {
            var animal = await _context.Animals.FindAsync(id);
            if (animal != null)
            {
                animal.AnimalGender = null;
                animal.AnimalType = null;
            }
            return animal;
        }

        public async Task<List<AnimalType>> GetAnimalTypes()
        {
            return await _context.AnimalTypes.ToListAsync();
        }

        public async Task<List<Animal>> GetMyAnimals(int userid)
        {
            List<Animal> animals = new List<Animal>();
            animals = await _context.Animals.Where(p => p.UserId == userid).ToListAsync();
            foreach (var animal in animals)
            {
                animal.AnimalGender = null;
                animal.AnimalType = null;
            }
            return animals;
        }


        public async Task<Response> UpdateAnimal(UpdAnimalModel model, int userid)
        {
            try
            {
                var animal = await _context.Animals.FirstOrDefaultAsync(p => p.Id == model.Id && p.UserId == userid);
                if (animal == null)
                {
                    return new Response { Status = "error", Message = "Не найден!" };
                }
                animal.Name = model.Name;
                animal.Breed = model.Breed;
                animal.Color = model.Color;
                animal.Age = model.Age;
                animal.AnimalGenderId = model.AnimalGenderId;
                animal.AnimalTypeId = model.AnimalTypeId;
                if (model.image != null)
                {
                    animal.Image = await UploadImage(animal.Id, model.image);
                }
                await Save();
                return new Response { Status = "success", Message = "Успешно изменен!" };
            }
            catch (Exception ex)
            {
                return new Response { Status = "error", Message = ex.Message };
            }
        }

        public async Task<Response> Delete(int id, int userid)
        {
            var animal = await _context.Animals.FirstOrDefaultAsync(p => p.Id == id && p.UserId == userid);
            if (animal == null)
            {
                return new Response { Status = "error", Message = "Не найден!" };
            }
            string path = Path.GetFullPath("Resources/Images/Animals/" + id);
            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
            }
            var chats = await _context.Chats.Where(p => p.FromAnimalId == id || p.ToAnimalId == id).ToListAsync();
            _context.RemoveRange(chats);
            _context.Animals.Remove(animal);
            await Save();
            return new Response { Status = "success", Message = "Успешно удален!" };
        }


        private async Task<int> Save()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
