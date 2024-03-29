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
        public async Task<IActionResult> SendFeedbackAsync(SendFeedbackDto request)
        {
            var sent = await _service.SendFeedbackAsync(request);
            return sent ? Ok() : BadRequest();
        }
    }
}
