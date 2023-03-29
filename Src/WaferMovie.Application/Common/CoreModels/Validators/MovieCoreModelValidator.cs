namespace WaferMovie.Application.Common.CoreModels.Validators;

public abstract class MovieCoreModelValidator<T> : AbstractValidator<T> where T : MovieCoreModel
{
    public MovieCoreModelValidator()
    {
        RuleFor(r => r.Title).NotEmpty()
            .WithMessage(m => $"{nameof(m.Title)} is required.")
            .MaximumLength(100)
            .WithMessage(m => $"{nameof(m.Title)} can not be longer than 100 characters");

        RuleFor(r => r.Description).MaximumLength(500)
            .WithMessage(m => $"{nameof(m.Description)} can not be longer than 100 characters");
    }
}