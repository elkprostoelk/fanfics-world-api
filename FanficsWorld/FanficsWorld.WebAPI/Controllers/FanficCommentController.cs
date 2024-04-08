using FanficsWorld.Common.DTO;
using FanficsWorld.Services.Interfaces;
using FanficsWorld.WebAPI.Extensions;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FanficsWorld.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FanficCommentController : ControllerBase
    {
        private readonly IFanficCommentService _service;
        private readonly IValidator<SentFanficCommentDto> _sentFanficCommentValidator;

        public FanficCommentController(
            IFanficCommentService service,
            IValidator<SentFanficCommentDto> sentFanficCommentValidator)
        {
            _service = service;
            _sentFanficCommentValidator = sentFanficCommentValidator;
        }

        [HttpGet("all/{fanficId:long}")]
        public async Task<IActionResult> GetFanficCommentsAsync(long fanficId)
        {
            var fanficComments = await _service.GetFanficCommentsAsync(fanficId, User.GetUserId());
            return Ok(fanficComments);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> SendFanficCommentAsync(SentFanficCommentDto sentFanficCommentDto)
        {
            var validationResult = await _sentFanficCommentValidator.ValidateAsync(sentFanficCommentDto);
            if (!validationResult.IsValid)
            {
                validationResult.AddToModelState(ModelState);
                return BadRequest(ModelState);
            }

            var addingResult = await _service.SendFanficCommentAsync(sentFanficCommentDto, User.GetUserId());
            return addingResult.IsSuccess
                ? NoContent()
                : BadRequest(addingResult);
        }

        [Authorize]
        [HttpPost("{commentId:long}")]
        public async Task<IActionResult> SetFanficCommentReactionAsync(long commentId, [FromQuery] bool? userLiked)
        {
            var fanficCommentReactionResult = await _service.SetFanficCommentReactionAsync(commentId, userLiked, User.GetUserId());

            return fanficCommentReactionResult.IsSuccess
                ? NoContent()
                : BadRequest(fanficCommentReactionResult);
        }
    }
}
