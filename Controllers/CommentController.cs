using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ZooMag.DTOs.Comment;
using ZooMag.Services.Interfaces;
using ZooMag.ViewModels;

namespace ZooMag.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _commentService;

        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        [HttpGet]
        public async Task<IActionResult> GetProductItemComments([FromQuery]int productItemId)
        {
            var response = await _commentService.GetProductItemReviewsAsync(productItemId);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody]CreateCommentRequest request)
        {
            Response response = await _commentService.CreateAsync(request);
            return Created("Comment",response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            List<CommentResponse> response = await _commentService.GetAllAsync();
            return Ok(response);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] int id)
        {
            Response response = await _commentService.DeleteAsync(id);
            return Created("Comment",response);
        }
    }
}