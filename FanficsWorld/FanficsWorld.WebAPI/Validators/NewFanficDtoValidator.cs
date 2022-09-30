using FanficsWorld.Common.DTO;
using FanficsWorld.DataAccess.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FanficsWorld.WebAPI.Validators;

public class NewFanficDtoValidator : AbstractValidator<NewFanficDTO>
{
    public NewFanficDtoValidator(UserManager<User> userManager)
    {
        RuleFor(dto => dto.Title)
            .NotEmpty().WithMessage("Fanfic title must be provided!")
            .Length(1, 30).WithMessage("Fanfic title must be from 1 to 30 symbols!");

        RuleFor(dto => dto.Annotation)
            .MaximumLength(100).WithMessage("Fanfic annotation must not be more than 100 symbols!");

        RuleFor(dto => dto.Text)
            .NotEmpty().WithMessage("Fanfic text must be provided!");

        When(dto => dto.CoauthorIds is not null, () =>
        {
            RuleFor(dto => dto.CoauthorIds)
                .Must(ids =>
                    ids.All(id => userManager.Users.Any(u => u.Id == id)));
        });
    }
}