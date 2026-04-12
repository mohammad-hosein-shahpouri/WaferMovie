namespace WaferMovie.Domain.Models;

public class EmailMessage<TBody>(string from, string to, string subject, TBody body)
{
    public string From { get; set; } = from;
    public string To { get; set; } = to;
    public string Subject { get; set; } = subject;
    public TBody Body { get; set; } = body;
}