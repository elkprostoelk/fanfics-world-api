using FanficsWorld.Common.DTO;
using FanficsWorld.Services.Interfaces;
using FluentValidation;

namespace FanficsWorld.WebAPI.Validators;

public class AddTagsDtoValidator : AbstractValidator<AddTagsDto>
{
    public AddTagsDtoValidator(ITagService tagService)
    {
        RuleFor(dto => dto.TagIds)
            .NotEmpty().WithMessage("You must provide any tag IDs to add!")
            .MustAsync(tagService.ContainsAllAsync)
            .WithMessage("Some of provided IDs do not exist!");
    }
}