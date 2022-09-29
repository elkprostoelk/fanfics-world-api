using FanficsWorld.Common.DTO;
using FluentValidation;

namespace FanficsWorld.WebAPI.Validators;

public class ChangePasswordDtoValidator : AbstractValidator<ChangePasswordDTO>
{
    public ChangePasswordDtoValidator()
    {
        RuleFor(dto => dto.CurrentPassword)
            .NotEmpty().WithMessage("Password must be specified!")
            .MinimumLength(8).WithMessage("Password must be 8 symbols or more!")
            .Matches(@"^\S+$").WithMessage("Password must not contain any spaces!");
        
        RuleFor(dto => dto.NewPassword)
            .NotEqual(dto => dto.CurrentPassword).WithMessage("New password must differ from the old one!")
            .NotEmpty().WithMessage("Password must be specified!")
            .MinimumLength(8).WithMessage("Password must be 8 symbols or more!")
            .Matches(@"^\S+$").WithMessage("Password must not contain any spaces!");
    }
}