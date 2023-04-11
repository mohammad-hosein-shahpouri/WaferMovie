namespace WaferMovie.Application.Common.Interfaces;

public interface ILocalizationService
{
    string FromSharedResources(string text);

    string FromValidationResources(string text);

    string FromPropertyResources(string text);
}