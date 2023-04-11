using Microsoft.Extensions.Localization;
using WaferMovie.Application.Common.Interfaces;
using WaferMovie.Infrastructure.Localization.Resources;

namespace WaferMovie.Infrastructure.Services;

public class LocalizationService : ILocalizationService
{
    private readonly IStringLocalizer sharedResourcesLocalizer;
    private readonly IStringLocalizer validationResourcesLocalizer;

    public LocalizationService(IStringLocalizerFactory factory)
    {
        var sharedResourcesType = typeof(SharedResources);
        var validationResourcesType = typeof(ValidationResources);

        sharedResourcesLocalizer = factory.Create(sharedResourcesType);
        validationResourcesLocalizer = factory.Create(validationResourcesType);
    }

    public string FromSharedResources(string text) => sharedResourcesLocalizer[text];

    public string FromValidationResources(string text) => validationResourcesLocalizer[text];
}