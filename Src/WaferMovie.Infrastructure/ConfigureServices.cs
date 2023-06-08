using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using StackExchange.Redis;
using System.Security.Claims;
using System.Text;
using WaferMovie.Application.Common.Interfaces;
using WaferMovie.Domain.Entities;
using WaferMovie.Infrastructure.Persistence;
using WaferMovie.Infrastructure.Services;

namespace WaferMovie.Infrastructure;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options => options.UseLazyLoadingProxies()
            .UseNpgsql(configuration.GetConnectionString("postgres")));

        services.AddRedis(configuration);

        services.AddDependencyInjection();

        services.AddIdentity<User, Domain.Entities.Role>()
            .AddDefaultTokenProviders()
         .AddEntityFrameworkStores<ApplicationDbContext>();

        services.AddLocalization();

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
            {
                var secretKey = Encoding.UTF8.GetBytes(configuration.GetValue<string>("Auth:JwtBearer:SecretKey")!);
                // var encryptionkey = Encoding.UTF8.GetBytes(configuration.GetValue<string>("Auth:JwtBearer:EncryptionKey"));

                var validationParameters = new TokenValidationParameters
                {
                    ClockSkew = TimeSpan.Zero, // default: 5 min
                    RequireSignedTokens = true,

                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(secretKey),

                    RequireExpirationTime = false,
                    ValidateLifetime = false,

                    ValidAudience = configuration.GetValue<string>("Auth:JwtBearer:Audience"),
                    ValidIssuer = configuration.GetValue<string>("Auth:JwtBearer:Issuer"),

                    SaveSigninToken = true,
                    //  TokenDecryptionKey = new SymmetricSecurityKey(encryptionkey),

                    NameClaimType = ClaimTypes.Name
                };

                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = validationParameters;
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        context.Token = context.Request.Headers[HeaderNames.Authorization];
                        return Task.CompletedTask;
                    }
                };
            });

        services.AddAuthorization();

        return services;
    }

    private static IServiceCollection AddDependencyInjection(this IServiceCollection services)
    {
        services.AddScoped<IApplicationDbContext, ApplicationDbContext>()
            .AddScoped<ITokenServices, TokenServices>()
            .AddScoped<IEmailServices, EmailServices>()
            .AddSingleton<ILocalizationService, LocalizationService>();

        return services;
    }

    public static IServiceCollection AddRedis(this IServiceCollection services, IConfiguration configuration)
    {
        var redisPort = configuration["Redis:Port"];
        var redisHost = configuration["Redis:Host"];
        var redisPassword = configuration["Redis:Password"];
        var redisDatabase = configuration.GetValue<int>("Redis:Database");
        var connectionString = $"{redisHost}:{redisPort},password={redisPassword}";

        services.AddScoped(cfg => ConnectionMultiplexer.Connect(connectionString).GetDatabase(redisDatabase));

        return services;
    }
}