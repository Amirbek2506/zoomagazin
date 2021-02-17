using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ZooMag.Models;
using ZooMag.Models.ViewModels.Measures;
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

        [HttpGet]
        [Route("fetchbyid/{id}")]
        public IActionResult GetMeasureById(int id)
        {
            Measure measure = _measureService.FetchById(id);
            if (measure == null)
            {
                return BadRequest(new Response { Status = "Error", Message = "Измерение не найдено!" });
            }
            return Ok(measure);
        }

        [HttpPost]
        [Route("create")]
        [Authorize(Roles = "Администратор")]
        public async Task<IActionResult> CreateMeasure([FromForm] InpMeasureModel measureModel)
        {
            var ress = await _measureService.Create(measureModel);
            if (ress.Status == "success")
            {
                return Ok(ress);
            }
            return BadRequest(ress);

        }

        [HttpPut]
        [Route("update")]
        [Authorize(Roles = "Администратор")]
        public async Task<IActionResult> UpdateMeasure([FromForm] Measure measure)
        {
            var ress = await _measureService.Update(measure);
            if (ress.Status == "success")
            {
                return Ok(ress);
            }
            return BadRequest(ress);
        }


        [HttpDelete]
        [Route("delete")]
        [Authorize(Roles = "Администратор")]
        public async Task<IActionResult> DeleteMeasure([FromForm] int id)
        {
            var ress =await _measureService.Delete(id);
            if (ress.Status == "success")
            {
                return Ok(ress);
            }
            return BadRequest(ress);
        }
    }
}