using Carter;
using LogisticCompany.data;
using LogisticCompany.models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.OpenApi;

var builder = WebApplication.CreateBuilder(args);
var connectionString =
(
    $" Host={Environment.GetEnvironmentVariable("HOST")};" +
    $" Database={Environment.GetEnvironmentVariable("DATABASE")};" +
    $" Username={Environment.GetEnvironmentVariable("USER")};" +
    $" Password={Environment.GetEnvironmentVariable("PASSWORD")};" +
    $" SearchPath={Environment.GetEnvironmentVariable("SEARCHPATH")};"
);

builder.Services.AddDbContext<LogisticcompanyContext>(options =>
    {
        options.UseNpgsql(connectionString);
    }
);

builder.Services.AddAuthorization();
builder.Services.AddIdentityApiEndpoints<IdentityUser>()
    .AddEntityFrameworkStores<LogisticcompanyContext>();
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.WebHost.ConfigureKestrel((context, options) =>
{
    options.ListenAnyIP(7028, listenOptions =>
    {
        listenOptions.Protocols = HttpProtocols.Http1AndHttp2AndHttp3;
        listenOptions.UseHttps();
    });
});
builder.Services.AddCarter();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Add database context
var app = builder.Build();
app.MapIdentityApi<IdentityUser>();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};
app.MapGet("/weatherforecast", Results<Ok<WeatherForecast[]>, UnauthorizedHttpResult> (int num) =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
            new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = summaries[Random.Shared.Next(summaries.Length)]
            })
        .ToArray();
    return num == 1 ? TypedResults.Unauthorized() : TypedResults.Ok(forecast);
}).WithName("GetWeatherForecast").WithOpenApi();
app.MapCarter();
app.Run();