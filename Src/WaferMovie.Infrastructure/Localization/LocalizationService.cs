using Microsoft.Extensions.Localization;
using WaferMovie.Application.Common.Interfaces;
using WaferMovie.Infrastructure.Localization.Resources;

namespace WaferMovie.Infrastructure.Services;

public class LocalizationService : ILocalizationService
{
    private readonly IStringLocalizer sharedResourcesLocalizer;
    private readonly IStringLocalizer validationResourcesLocalizer;
    private readonly IStringLocalizer propertyResourcesLocalizer;

    public LocalizationService(IStringLocalizerFactory factory)
    {
        var sharedResourcesType = typeof(SharedResources);
        var validationResourcesType = typeof(ValidationResources);
        var propertyResourcesType = typeof(PropertyResources);

        sharedResourcesLocalizer = factory.Create(sharedResourcesType);
        validationResourcesLocalizer = factory.Create(validationResourcesType);
        propertyResourcesLocalizer = factory.Create(propertyResourcesType);
    }

    public string FromSharedResources(string text) => sharedResourcesLocalizer[text];

    public string FromValidationResources(string text) => validationResourcesLocalizer[text];

    public string FromPropertyResources(string text) => propertyResourcesLocalizer[text];
}