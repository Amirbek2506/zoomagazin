using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZooMag.Services.Interfaces;
using ZooMag.DTOs.AdditionalServ;
using ZooMag.ViewModels;

namespace ZooMag.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AdditionalServController : Controller
    {        
        private readonly IAdditionalServService _additionalServiсe;
         
        public AdditionalServController(IAdditionalServService service)
        {
            _additionalServiсe = service;
        }

        [HttpPost]
        [Route("create")]
       // [Authorize(Roles = "Администратор")]
        public async Task<IActionResult> CreateAdditionalServ([FromForm]CreateAdditionalServRequest request)
        {
            var response = await _additionalServiсe.CreateAdditionalService(request);
            return Created("AdditionalServ",response);
        }

        [HttpGet]
        [Route("getAll")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _additionalServiсe.GetAllAdditionalServ();
            return Ok(result);
        }

        [HttpGet]
        [Route("get")]
        public async Task<IActionResult> GetAdditionalServ(int Id)
        {
            var result = await  _additionalServiсe.GetAdditionalServ(Id);
            if (result == null)
                return BadRequest(new Response {Status = "error",Message = "Данные об услуге не нейдены!"});
            return Ok(result);
        }

        [HttpPut]
        [Route("update")]
        //[Authorize(Roles = "Администратор")]
        public async Task<IActionResult> UpdateAdditionalServ([FromForm]  UpdateAdditionalServRequest request)
        {
            var result = await _additionalServiсe.UpdateAdditionalServ(request);
            return Created("AdditionalServ",result);
        }

        [HttpPost]
        [Route("delete")]
        //[Authorize(Roles = "Администратор")]
        public async Task<IActionResult> Delete(int Id)
        {           
            var response = await _additionalServiсe.DeleteAdditionalServ(Id);
            return Created("AdditionalServ", response);
        }
        
        [HttpPost]
        [Route("deleteImage")]
        public async Task<IActionResult> DeleteImage(int imageId)
        {           
            var response = await _additionalServiсe.DeleteServImage(imageId);
            return Created("AdditionalServ", response);
        }

        

    }
}