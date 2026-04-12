using System.Net;
using System.Net.Mail;
using WaferMovie.Domain.Models;

namespace WaferMovie.Infrastructure.Services;

public class EmailServices(IOptions<EmailOptions> options) : IEmailServices
{
    private readonly EmailOptions options = options.Value;
    public async Task SendAsync(EmailMessage<string> message)
    {
        using var client = new SmtpClient();
        var credential = new NetworkCredential
        {
            UserName = options.UserName,
            Password = options.Password,
        };

        client.Credentials = credential;
        client.Host = options.Host;
        client.Port = options.Port;
        client.EnableSsl = options.EnableSsl;

        MailMessage emailMessage = new(message.From, message.To, message.Subject, message.Body)
        {
            IsBodyHtml = false
        };

        await client.SendMailAsync(emailMessage);
    }
}