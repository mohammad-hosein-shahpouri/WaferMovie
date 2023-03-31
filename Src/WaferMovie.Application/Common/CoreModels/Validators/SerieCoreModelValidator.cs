﻿namespace WaferMovie.Application.Common.CoreModels.Validators;

public class SerieCoreModelValidator<T> : AbstractValidator<T> where T : SerieCoreModel
{
    public SerieCoreModelValidator()
    {
        RuleFor(r => r.Title).NotEmpty()
                .WithMessage(m => $"{nameof(m.Title)} is required.")
                .MaximumLength(100)
                .WithMessage(m => $"{nameof(m.Title)} can not be longer than 100 characters");

        RuleFor(r => r.Description).MaximumLength(500)
                .WithMessage(m => $"{nameof(m.Description)} can not be longer than 100 characters");
    }
}