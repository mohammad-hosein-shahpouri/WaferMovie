namespace WaferMovie.Application.Groups.Commands.CreateGroup;

public class CreateGroupCommandValidator : GroupCoreModelValidator<CreateGroupCommand>
{
    public CreateGroupCommandValidator(ILocalizationService localizationService) : base(localizationService)
    { }
}