using FabricPlatform.API.Quality.Application.Internal.CommandHandlers;
using FabricPlatform.API.Quality.Application.Internal.QueryHandlers;
using FabricPlatform.API.Quality.Domain.Repositories;
using FabricPlatform.API.Quality.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using FabricPlatform.API.Shared.Domain.Repositories;
using FabricPlatform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using FabricPlatform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseMySQL(connectionString);
});

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<IFabricInspectionRepository, FabricInspectionRepository>();
builder.Services.AddScoped<IDefectRepository, DefectRepository>();

builder.Services.AddScoped<CreateFabricInspectionCommandHandler>();
builder.Services.AddScoped<UpdateFabricInspectionParametersCommandHandler>();
builder.Services.AddScoped<CreateDefectCommandHandler>();
builder.Services.AddScoped<UpdateDefectCommandHandler>();

builder.Services.AddScoped<GetAllFabricInspectionsQueryHandler>();
builder.Services.AddScoped<GetFabricInspectionByIdQueryHandler>();
builder.Services.AddScoped<GetAllDefectsQueryHandler>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddControllers();

builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
    app.MapGet("/", () => Results.Redirect("/scalar/v1"));
}

app.UseCors("AllowAll");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();