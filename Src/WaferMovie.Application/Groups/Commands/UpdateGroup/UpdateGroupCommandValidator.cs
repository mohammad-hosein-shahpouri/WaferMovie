namespace WaferMovie.Application.Groups.Commands.UpdateGroup;

public class UpdateGroupCommandValidator : GroupCoreModelValidator<UpdateGroupCommand>
{
    public UpdateGroupCommandValidator(ILocalizationService localizationService) : base(localizationService)
    {
    }
}