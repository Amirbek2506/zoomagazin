using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ZooMag.DTOs.PetImage;
using ZooMag.Models;
using ZooMag.Models.ViewModels.Pets;
using ZooMag.Services.Interfaces;
using ZooMag.ViewModels;
using zoomagazin.DTOs.Pet;

namespace ZooMag.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PetController : ControllerBase
    {
        private readonly IPetsService _petsService;

        public PetController(IPetsService petsService)
        {
            this._petsService = petsService;
        }


        [HttpPost]
        [Route("create")]
      //  [Authorize(Roles = "Администратор")]
        public async Task<IActionResult> CreatePet([FromForm] CreatePetRequest model)
        {
            int petId = await _petsService.CreatePet(model);
            return Ok(new Response { Status = "Success", Message = "Питомец успешно добавлен!" });
        }

        [HttpPost]
        [Route("createImage")]
      //  [Authorize(Roles = "Администратор")]
        public async Task<IActionResult> CreatePetImage([FromForm] CreatePetImageRequest model)
        {
            int petId = await _petsService.CreatePetImage(model);
            return Ok(new Response { Status = "Success", Message = "Изображение успешно добавлено!" });
        }

        [HttpGet]
        [Route("getAll")]
        public async Task<IActionResult> GetAllPets()
        {          
            var result = await  _petsService.GetAllPets();
            if (result == null)
                return BadRequest(new Response {Status = "error",Message = "Список питомцев пуст!"});
            return Ok(result);
        }

        [HttpGet]
        [Route("getById")]
        public async Task<IActionResult> GetPetById(int Id)
        {          
            var result = await  _petsService.GetPet(Id);
            if (result == null)
                return BadRequest(new Response {Status = "error",Message = "Данные о товаре не нейдены!"});
            return Ok(result);
        }

        [HttpPost]
        [Route("delete")]
        public async Task<IActionResult> Delete(int Id)
        {           
            var response = await _petsService.DeletePet(Id);
            return Created("Pet", response);
        }
        
        [HttpPost]
        [Route("deleteImage")]
        public async Task<IActionResult> DeletePetImage(int petImageId)
        {           
            var response = await _petsService.DeletePetImage(petImageId);
            return Created("Pet", response);
        }

        [HttpPost]
        [Route("setMainImage")]
        public async Task<IActionResult> SetMainImage(int petId, int petImageId)
        {           
            var response = await _petsService.SetMainImage(petId, petImageId);
            return Created("Pet", response);
        }

        [HttpPost]
        [Route("update")]
        public async Task<IActionResult> Update([FromForm]UpdatePetRequest request)
        {
            var response = await  _petsService.UpdatePet(request);
            return Created("Pet", response);
        }
    }
}
