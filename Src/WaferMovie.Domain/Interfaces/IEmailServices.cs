using WaferMovie.Domain.Models;

namespace WaferMovie.Domain.Interfaces;

public interface IEmailServices
{
    Task SendAsync(EmailMessage<string> message);
}