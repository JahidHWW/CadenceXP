using CadenceXP.Api.Data;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using CadenceXP.Api.Services.Gpx;
using CadenceXP.Api.Services.Rides;
using CadenceXP.Api.Services.Xp;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(
            new JsonStringEnumConverter()
        );
    });

var connectionString =
    builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException(
        "Database connection string not found."
    );

builder.Services.AddDbContext<CadenceXpDbContext>(options =>
{
    options.UseNpgsql(connectionString);
});

builder.Services.AddScoped<GpxParser>();
builder.Services.AddScoped<RideProcessingService>();
builder.Services.AddScoped<XpService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend", policy =>
    {
        policy
            .WithOrigins("http://localhost:3000")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

app.UseCors("Frontend");

app.MapControllers();

app.Run();