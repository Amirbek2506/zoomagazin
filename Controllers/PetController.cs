using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
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


        // [HttpGet]
        // [Route("fetch")]
        // public async Task<IActionResult> GetPets(int offset=0, int limit=20,int categoryId=0)
        // {
        //     var pets = await _petsService.FetchPets(limit<1?1:limit,offset<1?0:offset,categoryId);
        //     int count = await _petsService.CountPets(categoryId);
        //     return Ok(new { count = count, pets = pets });
        // }
        

        // [HttpGet]
        // [Route("fetchbyid/{id}")]
        // public async Task<IActionResult> GetPetById(int id)
        // {
        //     var petModel = _petsService.FetchPetById(id);
        //     if (petModel == null)
        //     {
        //         return BadRequest(new Response { Status = "Error", Message = "Питомец не найден!" });
        //     }
        //     petModel.Images = await _petsService.FetchPetGaleriesByPetId(id);

        //     return Ok(petModel);
        // }


        // [HttpPut]
        // [Route("update")]
        // [Authorize(Roles = "Администратор")]
        // public async Task<IActionResult> UpdatePet([FromForm] UpdPetModel model)
        // {
        //     int petId = await _petsService.UpdatePet(model);
        //     if(petId==0)
        //     {
        //         return BadRequest(new Response { Status = "Error", Message = "Питомец не найден!" });
        //     }
           
        //     return Ok(new Response { Status = "Success", Message = "Питомец успешно изменен!" });
        // }


        // [HttpPost]
        // [Route("createimages")]
        // [Authorize(Roles = "Администратор")]
        // public async Task<IActionResult> CreateImages([FromForm]int petid, IFormFile[] Images)
        // {
        //     var petModel = _petsService.FetchPetById(petid);
        //     if (petModel == null)
        //     {
        //         return BadRequest(new Response { Status = "Error", Message = "Питомец не найден!" });
        //     }
        //     if (Images!=null)
        //     {
        //         await _petsService.CreatePetGaleries(petid, Images);
        //     }
        //     petModel.Images = await _petsService.FetchPetGaleriesByPetId(petid);

        //     return Ok(petModel);
        // }


        // [HttpPost]
        // [Route("setmainimage")]
        // [Authorize(Roles = "Администратор")]
        // public async Task<IActionResult> SetMainImage(int petid, int imageid)
        // {
        //     var ress = await _petsService.SetMainImage(petid,imageid);
        //     if(ress.Status == "success")
        //     {
        //         return Ok(ress);
        //     }
        //     return BadRequest(ress);
        // }


        // [HttpDelete]
        // [Route("delete/{id}")]
        // [Authorize(Roles = "Администратор")]
        // public async Task<IActionResult> DeletePet(int id)
        // {
        //     var ress = await _petsService.DeletePet(id);
        //     if (ress.Status == "success")
        //     {
        //         return Ok(ress);
        //     }
        //     return BadRequest(ress);
        // }


        // [HttpDelete]
        // [Route("deleteImage")]
        // [Authorize(Roles = "Администратор")]
        // public async Task<IActionResult> DeleteImage(int petid, int imageid)
        // {
        //     var ress = await _petsService.DeleteImage(imageid, petid);
        //     if (ress.Status == "success")
        //     {
        //         return Ok(ress);
        //     }
        //     return BadRequest(ress);
        // }
    }
}
