namespace WaferMovie.Application.Users.Commands.UpdateUser;

public class UpdateUserCommandValidator : UserCoreModelValidator<UpdateUserCommand>
{
    public UpdateUserCommandValidator(ILocalizationService localizationService) : base(localizationService)
    {
        RuleFor(x => x.Id).NotEmpty()
            .WithMessage(m => localizationService.FromValidationResources("{0} is required", localizationService.FromPropertyResources(nameof(m.Id))));
    }
}