using FabricPlatform.API.Alerts.Application.Internal.QueryServices;
using FabricPlatform.API.Alerts.Application.QueryServices;
using FabricPlatform.API.Alerts.Domain.Repositories;
using FabricPlatform.API.Alerts.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using FabricPlatform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Controllers
builder.Services.AddControllers();

// OpenAPI
builder.Services.AddOpenApi();

// Database Configuration
builder.Services.AddDbContext<AppDbContext>((serviceProvider, options) =>
{
    var connectionString = builder.Configuration
        .GetConnectionString("DefaultConnection");

    if (string.IsNullOrWhiteSpace(connectionString))
    {
        throw new InvalidOperationException(
            "Database connection string is not configured.");
    }

    options.UseMySQL(connectionString);
});

// Alerts - Dependency Injection
builder.Services.AddScoped<IAlertRepository, AlertRepository>();

builder.Services.AddScoped<IAlertQueryService, AlertQueryService>();

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("FabricWebApplication", policy =>
    {
        policy.WithOrigins("http://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

// OpenAPI
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// HTTP Pipeline
app.UseHttpsRedirection();

app.UseCors("FabricWebApplication");

app.UseAuthorization();

app.MapControllers();

app.Run();