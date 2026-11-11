using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Scalar.AspNetCore;

using FabricPlatform.API.Batches.Application.CommandServices;
using FabricPlatform.API.Batches.Application.Internal.CommandServices;
using FabricPlatform.API.Batches.Application.Internal.QueryServices;
using FabricPlatform.API.Batches.Application.QueryServices;
using FabricPlatform.API.Batches.Domain.Repositories;
using FabricPlatform.API.Batches.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using FabricPlatform.API.Shared.Domain.Repositories;
using FabricPlatform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using FabricPlatform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Configuración de Base de Datos (MySQL)
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseMySQL(builder.Configuration.GetConnectionString("DefaultConnection")!);
});

// Unit of Work y Repositorios
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IBatchRepository, BatchRepository>();
builder.Services.AddScoped<IProductionOperatorRepository, ProductionOperatorRepository>();

// Servicios de Comandos (Command Services) - ¡Los 3 necesarios para el controlador!
builder.Services.AddScoped<ICreateBatchCommandService, BatchCommandService>();
builder.Services.AddScoped<IAssignOperatorToBatchCommandService, BatchCommandService>();
builder.Services.AddScoped<IUpdateBatchStageCommandService, BatchCommandService>();
builder.Services.AddScoped<IBatchOperationsCommandService, BatchCommandService>();
builder.Services.AddScoped<ICreateProductionOperatorCommandService, ProductionOperatorCommandService>();

// Servicios de Consultas (Query Services)
builder.Services.AddScoped<IBatchQueryService, BatchQueryService>();
builder.Services.AddScoped<IBatchTraceabilityQueryService, BatchQueryService>();
builder.Services.AddScoped<IProductionOperatorQueryService, ProductionOperatorQueryService>();

builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();