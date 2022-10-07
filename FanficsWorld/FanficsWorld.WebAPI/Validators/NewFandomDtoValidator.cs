using FanficsWorld.Common.DTO;
using FluentValidation;

namespace FanficsWorld.WebAPI.Validators;

public class NewFandomDtoValidator : AbstractValidator<NewFandomDto>
{
    public NewFandomDtoValidator()
    {
        RuleFor(dto => dto.Title)
            .NotEmpty().WithMessage("Title must not be empty!");
    }
}