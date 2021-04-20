using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZooMag.Models.Entity;
using ZooMag.Models.ViewModels.Animals;
using ZooMag.Models.ViewModels.Chats;
using ZooMag.Services.Interfaces;
using ZooMag.ViewModels;

namespace ZooMag.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class ChatController : ControllerBase
    {
        private readonly IChatsService _chatsService;

        public ChatController(IChatsService chatsService)
        {
            this._chatsService = chatsService;
        }

        [HttpGet]
        [Route("CountUnreadMessages")]
        public async Task<IActionResult> CountUnreadMessages(int animalid)
        {
            return Ok(await _chatsService.CountUnreadMessages(animalid));
        }


        [HttpGet]
        [Route("fetchUnreadAnimals")]
        public async Task<IActionResult> FetchUnreadAnimals(int animalid)
        {
            return Ok(await _chatsService.FetchUnreadAnimals(animalid));
        }

        
        [HttpGet]
        [Route("fetchAnimals")]
        public async Task<IActionResult> FetchAnimals(int animalid)
        {
            return Ok(await _chatsService.FetchAnimals(animalid));
        }


        [HttpGet]
        [Route("fetch")]
        public async Task<IActionResult> Fetch(int fromanimalid, int toanimalid)
        {
            return Ok(await _chatsService.Get(fromanimalid, toanimalid));
        }

        [HttpPost]
        [Route("send")]
        public async Task<IActionResult> Send([FromForm] InpChatModel model)
        {
            if (await _chatsService.Send(model, int.Parse(User.Claims.First(i => i.Type == "UserId").Value)))
            {
                return Ok();
            }
            return BadRequest();
        }

        [HttpDelete]
        [Route("deletemessage/{id}")]
        public async Task<IActionResult> DeleteMessage(int id)
        {
            var ress = await _chatsService.DeleteMessage(id, int.Parse(User.Claims.First(i => i.Type == "UserId").Value));
            if(ress.Status == "success")
            {
                return Ok(ress);
            }
            return BadRequest(ress);
        }



        [HttpDelete]
        [Route("delete")]
        public async Task<IActionResult> Delete(int fromanimalid, int toanimalid)
        {
            return Ok(await _chatsService.Delete(fromanimalid, toanimalid));
        }

       
    }
}
