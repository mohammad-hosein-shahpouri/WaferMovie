using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;
using WaferMovie.Application.Common.Interfaces;
using WaferMovie.Application.Common.Models;

namespace WaferMovie.Infrastructure.Services;

public class EmailServices : IEmailServices
{
    private readonly IConfiguration configuration;

    public EmailServices(IConfiguration configuration)
    {
        this.configuration = configuration;
    }

    public async Task SendAsync(EmailMessage<string> message)
    {
        using (var client = new SmtpClient())
        {
            var credential = new NetworkCredential
            {
                UserName = configuration["Email:Username"],
                Password = configuration["Email:Username"],
            };

            client.Credentials = credential;
            client.Host = configuration["Email:Host"]!;
            client.Port = int.Parse(configuration["Email:Port"]!);
            client.EnableSsl = bool.Parse(configuration["Email:EnableSsl"]!);

            MailMessage emailMessage = new(message.From, message.To, message.Subject, message.Body)
            {
                IsBodyHtml = false
            };

            await client.SendMailAsync(emailMessage);
        }
    }
}