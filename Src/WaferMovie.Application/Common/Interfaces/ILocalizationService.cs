namespace WaferMovie.Application.Common.Interfaces;

public interface ILocalizationService
{
    string FromSharedResources(string text);

    string FromSharedResources(string text, params object[] args);

    string FromValidationResources(string text);

    string FromValidationResources(string text, params object[] args);

    string FromPropertyResources(string text);

    string FromPropertyResources(string text, params object[] args);
}