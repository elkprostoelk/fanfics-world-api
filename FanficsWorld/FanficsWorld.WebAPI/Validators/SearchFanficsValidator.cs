﻿using FanficsWorld.Common.DTO;
using FluentValidation;

namespace FanficsWorld.WebAPI.Validators
{
    public class SearchFanficsValidator : AbstractValidator<SearchFanficsDto>
    {
        public SearchFanficsValidator()
        {
            When(dto => dto.SortOrder.HasValue, () =>
            {
                RuleFor(dto => dto.SortBy)
                    .NotNull()
                    .IsInEnum();

                RuleFor(dto => dto.SortOrder)
                    .IsInEnum();
            });
        }
    }
}
