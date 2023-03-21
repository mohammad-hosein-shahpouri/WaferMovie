namespace WaferMovie.Application.Common.Models;

public class EmailMessage<TBody>
{
    public EmailMessage(string from, string to, string subject, TBody body)
    {
        From = from;
        To = to;
        Subject = subject;
        Body = body;
    }

    public string From { get; set; }
    public string To { get; set; }
    public string Subject { get; set; }
    public TBody Body { get; set; }
}