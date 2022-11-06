using FanficsWorld.Common.DTO;
using FanficsWorld.DataAccess.Entities;
using FanficsWorld.DataAccess.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FanficsWorld.WebAPI.Validators;

public class NewFanficDtoValidator : AbstractValidator<NewFanficDto>
{
    public NewFanficDtoValidator(
        UserManager<User> userManager,
        IFandomRepository fandomRepository,
        ITagRepository tagRepository
        )
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

        When(dto => dto.FandomIds is not null, () =>
        {
            RuleFor(dto => dto.FandomIds)
                .MustAsync(async (ids, token) =>
                    await fandomRepository.ContainsAllAsync(ids, token));
        });
        
        When(dto => dto.TagIds is not null, () =>
        {
            RuleFor(dto => dto.TagIds)
                .MustAsync(async (ids, token) =>
                    await tagRepository.ContainsAllAsync(ids, token));
        });

        RuleFor(dto => dto.Direction)
            .NotNull().WithMessage("You have to choose a fanfic direction!")
            .IsInEnum();
        
        RuleFor(dto => dto.Origin)
            .NotNull().WithMessage("You have to choose a fanfic origin!")
            .IsInEnum();
        
        RuleFor(dto => dto.Rating)
            .NotNull().WithMessage("You have to choose a fanfic rating!")
            .IsInEnum();
    }
}