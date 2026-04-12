namespace WaferMovie.Domain.Options;

public class EmailOptions
{
    public const string CONFIG = "Email";

    public required string UserName { get; set; }
    public required string Password { get; set; }
    public required string Host { get; set; }
    public int Port { get; set; }
    public bool EnableSsl { get; set; }
}
