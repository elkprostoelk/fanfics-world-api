using FanficsWorld.Common.DTO;
using FluentValidation;

namespace FanficsWorld.WebAPI.Validators;

public class LoginUserDtoValidator : AbstractValidator<LoginUserDTO>
{
    public LoginUserDtoValidator()
    {
        RuleFor(dto => dto.Password)
            .NotEmpty().WithMessage("Password must be specified!")
            .MinimumLength(8).WithMessage("Password must be 8 symbols or more!")
            .Matches(@"^\S+$").WithMessage("Password must not contain any spaces!");
        
        RuleFor(dto => dto.Login)
            .NotEmpty().WithMessage("Login must be provided!")
            .MaximumLength(20).WithMessage("Login must be less than 20 symbols!");
    }
}