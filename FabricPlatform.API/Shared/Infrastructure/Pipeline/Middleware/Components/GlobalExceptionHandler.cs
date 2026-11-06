using System;
using System.Threading;
using System.Threading.Tasks;
using FabricPlatform.API.Shared.Resources;
using FabricPlatform.API.Shared.Resources.Errors;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace FabricPlatform.API.Shared.Infrastructure.Pipeline.Middleware.Components;

public class GlobalExceptionHandler(
    ILogger<GlobalExceptionHandler> logger,
    IProblemDetailsService problemDetailsService,
    IStringLocalizer<ErrorMessages> errorLocalizer,
    IStringLocalizer<CommonMessages> commonLocalizer)
    : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        if (exception is OperationCanceledException opEx)
        {
            logger.LogWarning(opEx, "Request was cancelled: {Message}", opEx.Message);
            var problemDetails = new ProblemDetails
            {
                Status = StatusCodes.Status409Conflict,
                Title = errorLocalizer["OperationCancelled"],
                Detail = errorLocalizer["OperationCancelled"],
                Instance = httpContext.Request.Path
            };

            httpContext.Response.StatusCode = StatusCodes.Status409Conflict;
            return await problemDetailsService.TryWriteAsync(new ProblemDetailsContext
            {
                HttpContext = httpContext,
                ProblemDetails = problemDetails,
                Exception = exception
            });
        }

        logger.LogError(exception, "An unhandled exception occurred: {Message}", exception.Message);

        var genericProblemDetails = new ProblemDetails
        {
            Status = StatusCodes.Status500InternalServerError,
            Title = commonLocalizer["InternalServerError"],
            Detail = errorLocalizer["GenericError"],
            Instance = httpContext.Request.Path
        };

        httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
        return await problemDetailsService.TryWriteAsync(new ProblemDetailsContext
        {
            HttpContext = httpContext,
            ProblemDetails = genericProblemDetails,
            Exception = exception
        });
    }
}