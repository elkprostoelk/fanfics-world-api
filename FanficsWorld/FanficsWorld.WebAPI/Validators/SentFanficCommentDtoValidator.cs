using FanficsWorld.Common.DTO;
using FluentValidation;

namespace FanficsWorld.WebAPI.Validators;

public class SentFanficCommentDtoValidator: AbstractValidator<SentFanficCommentDto>
{
    public SentFanficCommentDtoValidator()
    {
        RuleFor(dto => dto.FanficId)
            .NotEmpty()
            .WithMessage("You must specify the fanfic ID!");

        RuleFor(dto => dto.Comment)
            .NotEmpty()
            .MaximumLength(200);
    }
}