using AspNetCore.SpaServices.ViteDevelopmentServer;
using WaferMovie.Application;
using WaferMovie.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
var isDevelopment = builder.Environment.IsDevelopment();
// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddApplication()
    .AddInfrastructure(builder.Configuration);

builder.Services.AddApiVersioning(o =>
{
    o.AssumeDefaultVersionWhenUnspecified = true;
    o.DefaultApiVersion = new ApiVersion(1, 0);
    o.ReportApiVersions = true;
});
builder.Services
    .AddSpaStaticFiles(configuration => configuration.RootPath = "ClientApp/dist");

var app = builder.Build();

#region Localization

var supportedCultures = new[] { "en-US", "fa-IR" };
var localizationOptions = new RequestLocalizationOptions().SetDefaultCulture(supportedCultures[0])
    .AddSupportedCultures(supportedCultures).AddSupportedUICultures(supportedCultures);
localizationOptions.ApplyCurrentCultureToResponseHeaders = true;

app.UseRequestLocalization(localizationOptions);

#endregion Localization

// Configure the HTTP request pipeline.
if (isDevelopment)
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseRouting();
app.UseHttpsRedirection();

app.UseSpaStaticFiles();
app.UseAuthorization();

#pragma warning disable ASP0014 // Suggest using top level route registrations
app.UseEndpoints(endpoints => endpoints.MapControllers());
#pragma warning restore ASP0014 // Suggest using top level route registrations

app.UseSpa(spa =>
{
    spa.Options.SourcePath = "ClientApp";
    //spa.Options.DevServerPort = 4173;

    if (isDevelopment) spa.UseViteDevelopmentServer("dev");
});

app.Run();