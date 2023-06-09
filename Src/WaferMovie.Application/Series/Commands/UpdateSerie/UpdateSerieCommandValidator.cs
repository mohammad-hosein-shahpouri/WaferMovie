using WaferMovie.Application.Movies.Commands.UpdateMovie;

namespace WaferMovie.Application.Series.Commands.UpdateSerie;

public class UpdateSerieCommandValidator : SerieCoreModelValidator<UpdateSerieCommand>
{
    public UpdateSerieCommandValidator(ILocalizationService localization) : base(localization)
    {
        RuleFor(r => r.Id).NotEmpty()
            .WithMessage(m => localization.FromValidationResources("{0} is required", localization.FromPropertyResources(nameof(m.Id))));
    }
}