using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;


namespace WaferMovie.Infrastructure.Services;

public class TokenServices(IOptions<JwtOptions> jwtOptions) : ITokenServices
{
    private readonly JwtOptions jwtOptions = jwtOptions.Value;
    public string GenerateJwtAsync(User user)
    {
        var secretKey = Encoding.UTF8.GetBytes(jwtOptions.SecretKey);
        var signingCredentials = new SigningCredentials(
            new SymmetricSecurityKey(secretKey), SecurityAlgorithms.HmacSha256Signature);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            CompressionAlgorithm = jwtOptions.CompressionAlgorithm,
            Issuer = jwtOptions.CompressionAlgorithm,
            Audience = jwtOptions.Audience,
            IssuedAt = DateTime.UtcNow,
            NotBefore = DateTime.UtcNow,
            Expires = DateTime.UtcNow.AddDays(31),
            SigningCredentials = signingCredentials,
            Subject = new ClaimsIdentity(GetClaims(user))
        };

        if (Convert.ToBoolean(jwtOptions.UseEncryptionKey))
        {
            ArgumentNullException.ThrowIfNullOrWhiteSpace(nameof(jwtOptions.UseEncryptionKey));
            var encryptionKey = Encoding.UTF8.GetBytes(jwtOptions.EncryptionKey!);
            var encryptingCredentials = new EncryptingCredentials(
                new SymmetricSecurityKey(encryptionKey), SecurityAlgorithms.Aes128KW,
                SecurityAlgorithms.Aes128CbcHmacSha256);

            tokenDescriptor.EncryptingCredentials = encryptingCredentials;
        }

        var tokenHandler = new JwtSecurityTokenHandler();
        var securityToken = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(securityToken);
    }

    private static List<Claim> GetClaims(User user)
    {
        List<Claim> claims = [
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.NormalizedUserName!),
            new(ClaimTypes.Email, user.Email!),
            new(new ClaimsIdentityOptions().SecurityStampClaimType, user.SecurityStamp ?? string.Empty),
            new(JwtRegisteredClaimNames.Jti, Guid.CreateVersion7().ToString())
        ];

        if (user.BirthDate is not null)
            claims.Add(new(ClaimTypes.DateOfBirth, user.BirthDate.Value.ToString("YYYY-MM-DD")));

        return claims;
    }
}