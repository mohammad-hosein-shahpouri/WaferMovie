namespace WaferMovie.Domain.Options;

public class JwtOptions
{
    public const string CONFIG = "Auth:JwtBearer";

    public required string SecretKey { get; set; }
    public required string CompressionAlgorithm { get; set; }
    public required string Issuer { get; set; }
    public required string Audience { get; set; }
    public bool UseEncryptionKey { get; set; }
    public string? EncryptionKey { get; set; }
}
