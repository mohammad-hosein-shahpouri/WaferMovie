using WaferMovie.Domain.Common;

namespace WaferMovie.Application.Common.Behaviors;

public class LocalizationPipelineBehavior<TRequest, TResponse>(ILocalizationService localizationService) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : ApiResponse
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var response = await next(cancellationToken);
        if (response.Messages.Count == 0)
            response.Messages.Add(localizationService.FromSharedResources(response.Status.ToString()));
        else if (response.Messages.Count == 1 && response.Messages[0] == response.Status.ToString())
            response.Messages = [
                localizationService.FromSharedResources(response.Status.ToString())
            ];

        return response;
    }
}