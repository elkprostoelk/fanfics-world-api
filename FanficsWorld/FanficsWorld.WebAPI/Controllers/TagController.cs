using FanficsWorld.Common.DTO;
using FanficsWorld.Services.Interfaces;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FanficsWorld.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TagController : ControllerBase
{
    private readonly ITagService _service;
    private readonly IValidator<NewTagDto> _newTagDtoValidator;

    public TagController(
        ITagService service,
        IValidator<NewTagDto> newTagDtoValidator)
    {
        _service = service;
        _newTagDtoValidator = newTagDtoValidator;
    }

    [Authorize(Roles = "Admin")]
    [HttpGet]
    [ProducesResponseType(typeof(List<ServicePagedResultDto<AdminPageTagDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllTags(string? searchByName = null, int page = 1, int itemsPerPage = 5) =>
        Ok(await _service.GetAllAsync(searchByName, page, itemsPerPage));

    [HttpGet("top10")]
    [ProducesResponseType(typeof(List<TagDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTop10Tags() =>
        Ok(await _service.GetTop10Async());

    [HttpGet("search")]
    [ProducesResponseType(typeof(List<TagDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTagsByTitle(string title) =>
        Ok(await _service.GetAllAsync(title));

    [HttpGet("{id:long}")]
    [ProducesResponseType(typeof(TagWithFanficsDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetTagWithFanfics(long id)
    {
        var tag = await _service.GetFullByIdAsync(id);
        if (tag is null)
        {
            return NotFound($"Tag {id} was not found!");
        }

        return Ok(tag);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ServiceResultDto), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateNewTag(NewTagDto newTagDto)
    {
        var validationResult = await _newTagDtoValidator.ValidateAsync(newTagDto);
        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState);
            return BadRequest(ModelState);
        }

        var creationResult = await _service.CreateAsync(newTagDto);

        return creationResult.IsSuccess
            ? StatusCode(201)
            : BadRequest(creationResult);
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:long}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ServiceResultDto), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteTag(long id)
    {
        var deleteResult = await _service.DeleteAsync(id);
        return deleteResult.IsSuccess
            ? NoContent()
            : BadRequest(deleteResult);
    }
}