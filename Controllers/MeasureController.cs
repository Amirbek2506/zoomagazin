using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ZooMag.Models;
using ZooMag.Services.Interfaces;
using ZooMag.ViewModels;

namespace ZooMag.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MeasureController : ControllerBase
    {
        private readonly IMeasuresService _measureService;

        public MeasureController(IMeasuresService measureService)
        {
            this._measureService = measureService;
        }

        [HttpGet]
        [Route("fetch")]
        public async Task<IActionResult> GetMeasures()
        {
            return Ok(await _measureService.Fetch());
        }

        //[HttpGet]
        //[Route("fetchbyid")]
        //public IActionResult GetMeasureById(int id)
        //{
        //    Measure measure = _measureService.FetchById(id);
        //    if (measure == null)
        //    {
        //        return StatusCode(
        //            StatusCodes.Status400BadRequest,
        //            new Response { Status = "Error", Message = "Измерение не найдено!" });
        //    }
        //    return Ok(measure);
        //}

        [HttpPost]
        [Route("create")]
        [Authorize(Roles = "Администратор")]
        public async Task<IActionResult> CreateMeasure([FromForm] string title)
        {
            if (String.IsNullOrEmpty(title))
                return StatusCode(StatusCodes.Status400BadRequest, new Response { Status = "Error", Message = "Invalid Category!" });

            _measureService.Create(title);
            await _measureService.Save();
            return Ok(new Response { Status = "Success", Message = "Измерение успешно добавлено!" });
        }

        [HttpPut]
        [Route("update")]
        [Authorize(Roles = "Администратор")]
        public async Task<IActionResult> UpdateMeasure([FromForm]int id, [FromForm]string title)
        {
            if (String.IsNullOrEmpty(title))
                return StatusCode(StatusCodes.Status400BadRequest, new Response { Status = "Error", Message = "Invalid Category!" });

            _measureService.Update(id, title);
            await _measureService.Save();
            return Ok(new Response { Status = "Success", Message = "Измерение успешно изменено!" });
        }


        [HttpDelete]
        [Route("delete")]
        [Authorize(Roles = "Администратор")]
        public async Task<IActionResult> DeleteMeasure([FromForm] int id)
        {
            _measureService.Delete(id);
            await _measureService.Save();
            return Ok(new Response { Status = "Success", Message = "Измерения успешно удалено!" });
        }
    }
}