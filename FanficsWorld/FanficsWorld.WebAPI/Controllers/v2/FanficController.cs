using FanficsWorld.Common.DTO;
using FanficsWorld.Services.Interfaces;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;

namespace FanficsWorld.WebAPI.Controllers.v2
{
    [Route("api/v2/[controller]")]
    [ApiController]
    public class FanficController : ControllerBase
    {
        private readonly IFanficService _service;
        private readonly IValidator<SearchFanficsDto> _searchFanficsValidator;

        public FanficController(
            IFanficService service,
            IValidator<SearchFanficsDto> searchFanficsValidator)
        {
            _service = service;
            _searchFanficsValidator = searchFanficsValidator;
        }

        [HttpGet]
        public async Task<IActionResult> SearchFanficsAsync([FromQuery] SearchFanficsDto searchFanficsDto)
        {
            var validationResult = await _searchFanficsValidator.ValidateAsync(searchFanficsDto);
            if (!validationResult.IsValid)
            {
                validationResult.AddToModelState(ModelState);
                return BadRequest(ModelState);
            }

            return Ok(await _service.SearchFanficsAsync(searchFanficsDto));
        }
    }
}
