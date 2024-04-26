using FanficsWorld.Common.DTO;
using FanficsWorld.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FanficsWorld.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeedbackController : ControllerBase
    {
        private readonly IFeedbackService _service;

        public FeedbackController(IFeedbackService service)
        {
            _service = service;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SendFeedback(SendFeedbackDto request)
        {
            var sentResult = await _service.SendFeedbackAsync(request);
            return sentResult.IsSuccess
                ? StatusCode(201)
                : BadRequest(sentResult);
        }
    }
}
