using WaferMovie.Application.Common.Models;

namespace WaferMovie.Application.Common.Interfaces;

public interface IEmailServices
{
    Task SendAsync(EmailMessage<string> message);
}