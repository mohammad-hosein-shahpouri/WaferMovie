namespace WaferMovie.Domain.Options;

public class RedisOptions
{
    public const string CONFIG = "Redis";

    public string Host { get; set; } = default!;
    public int Port { get; set; }
    public string Password { get; set; } = default!;
    public int Database { get; set; }
}