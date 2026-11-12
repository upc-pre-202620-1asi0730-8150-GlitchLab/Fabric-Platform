using FabricPlatform.API.Alerts.Application.Internal.QueryServices;
using FabricPlatform.API.Alerts.Application.QueryServices;
using FabricPlatform.API.Alerts.Domain.Repositories;
using FabricPlatform.API.Alerts.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using FabricPlatform.API.Machinery.Application.CommandServices;
using FabricPlatform.API.Machinery.Application.Internal.CommandServices;
using FabricPlatform.API.Machinery.Application.Internal.QueryServices;
using FabricPlatform.API.Machinery.Application.QueryServices;
using FabricPlatform.API.Machinery.Domain.Repositories;
using FabricPlatform.API.Machinery.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using FabricPlatform.API.Machinery.Resources;
using FabricPlatform.API.Quality.Application.Internal.CommandHandlers;
using FabricPlatform.API.Quality.Application.Internal.QueryHandlers;
using FabricPlatform.API.Quality.Domain.Repositories;
using FabricPlatform.API.Quality.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using FabricPlatform.API.Shared.Domain.Repositories;
using FabricPlatform.API.Shared.Infrastructure.Interfaces.AspNetCore.Configuration;
using FabricPlatform.API.Shared.Infrastructure.Interfaces.AspNetCore.OpenApi;
using FabricPlatform.API.Shared.Infrastructure.Mediator.Cortex.Configuration;
using FabricPlatform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using FabricPlatform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Interceptors;
using FabricPlatform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using FabricPlatform.API.Shared.Infrastructure.Pipeline.Middleware.Components;
using FabricPlatform.API.Shared.Resources;
using FabricPlatform.API.Shared.Resources.Errors;
using Cortex.Mediator.Commands;
using Cortex.Mediator.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Scalar.AspNetCore;
using ProblemDetailsFactory = FabricPlatform.API.Shared.Interfaces.Rest.ProblemDetails.ProblemDetailsFactory;
using FabricPlatform.API.Analytics.Application.Internal.QueryServices;
using FabricPlatform.API.Analytics.Application.QueryServices;
using FabricPlatform.API.Batches.Domain.Repositories;
using FabricPlatform.API.Batches.Infrastructure.Persistence.EntityFrameworkCore.Repositories;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.Services.AddControllers(options => options.Conventions.Add(new KebabCaseRouteNamingConvention()))
    .AddDataAnnotationsLocalization();

builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllPolicy",
        policy => policy.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});

builder.Services.AddDbContext<AppDbContext>((serviceProvider, options) =>
{
    var connectionStringTemplate = builder.Configuration.GetConnectionString("DefaultConnection");
    if (string.IsNullOrWhiteSpace(connectionStringTemplate))
        throw new InvalidOperationException("Database connection string is not set in the configuration.");

    var connectionString = Environment.ExpandEnvironmentVariables(connectionStringTemplate);
    if (string.IsNullOrWhiteSpace(connectionString))
        throw new InvalidOperationException("Database connection string is not set in the configuration.");

    options.UseMySQL(connectionString)
        .UseLoggerFactory(serviceProvider.GetRequiredService<ILoggerFactory>())
        .AddInterceptors(serviceProvider.GetRequiredService<DispatchDomainEventsInterceptor>())
        .EnableDetailedErrors();

    if (builder.Environment.IsDevelopment())
        options.EnableSensitiveDataLogging();
});

builder.Services.AddLocalization();

builder.Services.AddSingleton<IStringLocalizer<ErrorMessages>, StringLocalizer<ErrorMessages>>();
builder.Services.AddSingleton<IStringLocalizer<CommonMessages>, StringLocalizer<CommonMessages>>();
builder.Services.AddSingleton<IStringLocalizer<MachineryMessage>, StringLocalizer<MachineryMessage>>();

builder.Services.AddScoped<IBatchRepository, BatchRepository>();


builder.Services.AddScoped<IAnalyticsQueryService, AnalyticsQueryService>();
builder.Services.AddSingleton<ProblemDetailsFactory>();

builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer<BearerSecuritySchemeTransformer>();
    options.AddOperationTransformer<IamBearerSecurityOperationTransformer>();
});

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<DispatchDomainEventsInterceptor>();

// Machinery bounded context
builder.Services.AddScoped<IMachineRepository, MachineRepository>();
builder.Services.AddScoped<IMachineCommandService, MachineCommandService>();
builder.Services.AddScoped<IMachineQueryService, MachineQueryService>();

// Quality bounded context
builder.Services.AddScoped<IFabricInspectionRepository, FabricInspectionRepository>();
builder.Services.AddScoped<IDefectRepository, DefectRepository>();

builder.Services.AddScoped<CreateFabricInspectionCommandHandler>();
builder.Services.AddScoped<UpdateFabricInspectionParametersCommandHandler>();
builder.Services.AddScoped<CreateDefectCommandHandler>();
builder.Services.AddScoped<UpdateDefectCommandHandler>();

builder.Services.AddScoped<GetAllFabricInspectionsQueryHandler>();
builder.Services.AddScoped<GetFabricInspectionByIdQueryHandler>();
builder.Services.AddScoped<GetAllDefectsQueryHandler>();

// Alerts bounded context
builder.Services.AddScoped<IAlertRepository, AlertRepository>();
builder.Services.AddScoped<IAlertQueryService, AlertQueryService>();

builder.Services.AddScoped(typeof(ICommandPipelineBehavior<>), typeof(LoggingCommandBehavior<>));
builder.Services.AddScoped(typeof(ICommandPipelineBehavior<>), typeof(ExceptionHandlingCommandBehavior<>));
builder.Services.AddScoped(typeof(ICommandPipelineBehavior<>), typeof(TransactionCommandBehavior<>));

builder.Services.AddCortexMediator(
    [typeof(Program)]);

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<AppDbContext>();
    context.Database.Migrate();
}

app.UseExceptionHandler();

var supportedCultures = new[] { "en", "es" };
var localizationOptions = new RequestLocalizationOptions()
    .SetDefaultCulture(supportedCultures[0])
    .AddSupportedCultures(supportedCultures)
    .AddSupportedUICultures(supportedCultures);

app.UseRequestLocalization(localizationOptions);

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
    app.MapGet("/", () => Results.Redirect("/scalar/v1"));
}

app.UseCors("AllowAllPolicy");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
