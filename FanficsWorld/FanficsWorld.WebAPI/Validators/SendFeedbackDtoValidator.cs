using FanficsWorld.Common.DTO;
using FluentValidation;

namespace FanficsWorld.WebAPI.Validators;

public class SendFeedbackDtoValidator : AbstractValidator<SendFeedbackDto>
{
    public SendFeedbackDtoValidator()
    {
        RuleFor(dto => dto.Name)
            .MaximumLength(20);

        RuleFor(dto => dto.Email)
            .NotEmpty()
            .EmailAddress()
            .MaximumLength(256);

        RuleFor(dto => dto.Text)
            .NotEmpty()
            .MaximumLength(2000);
    }
}