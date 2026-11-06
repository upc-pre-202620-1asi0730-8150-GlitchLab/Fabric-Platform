using System;
using System.Net.Mime;
using System.Text.Json;
using System.Threading.Tasks;
using FabricPlatform.API.Shared.Resources;
using FabricPlatform.API.Shared.Resources.Errors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace FabricPlatform.API.Shared.Infrastructure.Pipeline.Middleware.Components;

[Obsolete("Use ASP.NET Core IExceptionHandler with GlobalExceptionHandler and app.UseExceptionHandler() instead.")]
public class GlobalExceptionHandlerMiddleware(
    RequestDelegate next,
    ILogger<GlobalExceptionHandlerMiddleware> logger,
    IStringLocalizer<ErrorMessages> errorLocalizer,
    IStringLocalizer<CommonMessages>
        commonLocalizer)
{
    private readonly IStringLocalizer<CommonMessages> _commonLocalizer = commonLocalizer;
    private readonly IStringLocalizer<ErrorMessages> _errorLocalizer = errorLocalizer;

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (OperationCanceledException ex)
        {
            logger.LogWarning(ex, "Request was cancelled: {Message}", ex.Message);
            await HandleOperationCanceledExceptionAsync(context, ex);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An unhandled exception occurred: {Message}", ex.Message);
            await HandleGenericExceptionAsync(context, ex);
        }
    }

    private async Task HandleOperationCanceledExceptionAsync(HttpContext context, OperationCanceledException exception)
    {
        context.Response.ContentType = MediaTypeNames.Application.Json;
        context.Response.StatusCode = StatusCodes.Status409Conflict;

        var problemDetails = new ProblemDetails
        {
            Status = StatusCodes.Status409Conflict,
            Title = _errorLocalizer["OperationCancelled"],
            Detail = _errorLocalizer["OperationCancelled"],
            Instance = context.Request.Path
        };

        var jsonOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        var result = JsonSerializer.Serialize(problemDetails, jsonOptions);

        await context.Response.WriteAsync(result);
    }

    private async Task HandleGenericExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = MediaTypeNames.Application.Json;
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;

        var problemDetails = new ProblemDetails
        {
            Status = StatusCodes.Status500InternalServerError,
            Title = _commonLocalizer["InternalServerError"],
            Detail = _errorLocalizer["GenericError"],
            Instance = context.Request.Path
        };

        var jsonOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        var result = JsonSerializer.Serialize(problemDetails, jsonOptions);

        await context.Response.WriteAsync(result);
    }
}