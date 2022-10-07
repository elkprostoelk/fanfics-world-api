using FanficsWorld.Common.DTO;
using FluentValidation;

namespace FanficsWorld.WebAPI.Validators;

public class RegisterUserDtoValidator : AbstractValidator<RegisterUserDto>
{
    public RegisterUserDtoValidator()
    {
        RuleFor(dto => dto.Email)
            .EmailAddress().WithMessage("E-mail address should be valid!")
            .MaximumLength(256).WithMessage("E-mail should be less than 256 symbols!");

        RuleFor(dto => dto.Age)
            .GreaterThan((ushort)0).WithMessage("Age must be positive!");

        RuleFor(dto => dto.Password)
            .NotEmpty().WithMessage("Password must be specified!")
            .MinimumLength(8).WithMessage("Password must be 8 symbols or more!")
            .Matches(@"^\S+$").WithMessage("Password must not contain any spaces!");

        RuleFor(dto => dto.PhoneNumber)
            .Matches(@"^[\d\-\(\)+]+$").WithMessage("Phone number is invalid!")
            .MaximumLength(20).WithMessage("Phone number must not be more than 20 symbols!");

        RuleFor(dto => dto.UserName)
            .NotEmpty().WithMessage("User name must be provided!")
            .MaximumLength(20).WithMessage("User name must be less than 20 symbols!");

        RuleFor(dto => dto.Role)
            .NotEmpty().WithMessage("Role must be provided")
            .MaximumLength(256).WithMessage("Role name must be less than 256 symbols!");
    }
}