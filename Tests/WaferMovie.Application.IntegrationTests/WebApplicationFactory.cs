using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using System.Net;

namespace WaferMovie.Application.IntegrationTests;

public class WebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration(configurationBuilder =>
        {
            var integrationConfig = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();

            configurationBuilder.AddConfiguration(integrationConfig);
        });

        builder.ConfigureServices((builder, services) =>
        {
            services.Remove<IHttpContextAccessor>()
                .AddTransient<IHttpContextAccessor>(provider =>
                {
                    var httpContextAccessor = new HttpContextAccessor();
                    var httpContext = new DefaultHttpContext();
                    httpContext.Connection.LocalIpAddress = IPAddress.Any;
                    httpContext.Connection.RemoteIpAddress = IPAddress.Any;
                    httpContextAccessor.HttpContext = httpContext;

                    return httpContextAccessor;
                });

            services.Remove<DbContextOptions<ApplicationDbContext>>()
                .AddDbContext<ApplicationDbContext>((sp, options) =>
                    options.UseSqlite(builder.Configuration.GetConnectionString("sqlite"),
                         builder => builder.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));
        });
    }
}