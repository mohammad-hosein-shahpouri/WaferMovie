using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WaferMovie.Application.Common.Interfaces;
using WaferMovie.Domain.Entities;

namespace WaferMovie.Infrastructure.Services;

public class TokenServices : ITokenServices
{
    private readonly IConfiguration configuration;

    public TokenServices(IConfiguration configuration)
    {
        this.configuration = configuration;
    }

    public string GenerateJwtAsync(User user)
    {
        var secretKey = Encoding.UTF8.GetBytes(configuration["Auth:JWTBearer:SecretKey"]!);
        var signingCredentials = new SigningCredentials(
            new SymmetricSecurityKey(secretKey), SecurityAlgorithms.HmacSha256Signature);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            CompressionAlgorithm = configuration["Auth:JWTBearer:CompressionAlgorithm"],
            Issuer = configuration["Auth:JWTBearer:Issuer"],
            Audience = configuration["Auth:JWTBearer:Audience"],
            IssuedAt = DateTime.UtcNow,
            NotBefore = DateTime.UtcNow,
            Expires = DateTime.UtcNow.AddDays(31),
            SigningCredentials = signingCredentials,
            Subject = new ClaimsIdentity(GetClaims(user))
        };

        if (Convert.ToBoolean(configuration.GetSection("Auth:JWTBearer:UseEncryptionKey").Value))
        {
            var encryptionKey = Encoding.UTF8.GetBytes(configuration["Auth:JWTBearer:EncryptionKey"]!);
            var encryptingCredentials = new EncryptingCredentials(
                new SymmetricSecurityKey(encryptionKey), SecurityAlgorithms.Aes128KW,
                SecurityAlgorithms.Aes128CbcHmacSha256);

            tokenDescriptor.EncryptingCredentials = encryptingCredentials;
        }

        var tokenHandler = new JwtSecurityTokenHandler();
        var securityToken = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(securityToken);
    }

    private IEnumerable<Claim> GetClaims(User user)
    {
        var claims = new List<Claim>
        {
            new("Developer", "Mohammad Hosein Shahpouri"),
            new(ClaimTypes.Name, user.NormalizedUserName!),
            new(new ClaimsIdentityOptions().SecurityStampClaimType, user.SecurityStamp ?? string.Empty),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        return claims;
    }
}