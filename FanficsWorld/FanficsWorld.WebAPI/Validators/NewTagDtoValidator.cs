using FanficsWorld.Common.DTO;
using FluentValidation;

namespace FanficsWorld.WebAPI.Validators;

public class NewTagDtoValidator : AbstractValidator<NewTagDto>
{
    public NewTagDtoValidator() =>
        RuleFor(dto => dto.Name)
            .NotEmpty()
            .MaximumLength(50);
}