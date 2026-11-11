using FabricPlatform.API.Machinery.Domain.Model;
using FabricPlatform.API.Machinery.Domain.Model.Aggregates;
using FabricPlatform.API.Machinery.Domain.Model.Entities;
using FabricPlatform.API.Shared.Application.Model;
using FabricPlatform.API.Shared.Interfaces.Rest.ProblemDetails;
using FabricPlatform.API.Shared.Resources.Errors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace FabricPlatform.API.Machinery.Interfaces.Rest.Transform;

public static class MachineryActionResultAssembler
{
    private static int ToStatusCodeFromMachineryError(MachineryError error)
    {
        return error switch
        {
            MachineryError.MachineNotFound => StatusCodes.Status404NotFound,
            MachineryError.DowntimeNotFound => StatusCodes.Status404NotFound,
            MachineryError.DuplicateMachineCode => StatusCodes.Status409Conflict,
            MachineryError.MachineAlreadyStopped => StatusCodes.Status409Conflict,
            MachineryError.DowntimeAlreadyClosed => StatusCodes.Status409Conflict,
            MachineryError.OperationCancelled => StatusCodes.Status409Conflict,
            MachineryError.DatabaseError => StatusCodes.Status500InternalServerError,
            MachineryError.InternalServerError => StatusCodes.Status500InternalServerError,
            _ => StatusCodes.Status400BadRequest
        };
    }

    private static IActionResult ToActionResultFromResult<T>(
        ControllerBase controller,
        Result<T> result,
        ProblemDetailsFactory problemDetailsFactory,
        Func<T, IActionResult> successAction)
    {
        if (result.IsSuccess) return successAction(result.Value!);

        var error = result.Error is MachineryError machineryError ? machineryError : MachineryError.InternalServerError;
        var statusCode = ToStatusCodeFromMachineryError(error);
        return problemDetailsFactory.CreateProblemDetails(controller, statusCode, result.Error, result.Message);
    }

    public static IActionResult ToActionResultFromRegisterMachineResult(
        ControllerBase controller,
        Result<Machine> result,
        IStringLocalizer<ErrorMessages> errorLocalizer,
        ProblemDetailsFactory problemDetailsFactory,
        Func<Machine, IActionResult> successAction)
    {
        return ToActionResultFromResult(controller, result, problemDetailsFactory, successAction);
    }

    public static IActionResult ToActionResultFromUpdateMachineStatusResult(
        ControllerBase controller,
        Result<Machine> result,
        IStringLocalizer<ErrorMessages> errorLocalizer,
        ProblemDetailsFactory problemDetailsFactory,
        Func<Machine, IActionResult> successAction)
    {
        return ToActionResultFromResult(controller, result, problemDetailsFactory, successAction);
    }

    public static IActionResult ToActionResultFromMachineDowntimeResult(
        ControllerBase controller,
        Result<MachineDowntime> result,
        IStringLocalizer<ErrorMessages> errorLocalizer,
        ProblemDetailsFactory problemDetailsFactory,
        Func<MachineDowntime, IActionResult> successAction)
    {
        return ToActionResultFromResult(controller, result, problemDetailsFactory, successAction);
    }

    public static IActionResult ToActionResultFromRegisterMaintenanceRecordResult(
        ControllerBase controller,
        Result<MaintenanceRecord> result,
        IStringLocalizer<ErrorMessages> errorLocalizer,
        ProblemDetailsFactory problemDetailsFactory,
        Func<MaintenanceRecord, IActionResult> successAction)
    {
        return ToActionResultFromResult(controller, result, problemDetailsFactory, successAction);
    }

    public static IActionResult ToActionResultFromGetMachineByIdResult(
        ControllerBase controller,
        Machine? machine,
        IStringLocalizer<ErrorMessages> errorLocalizer,
        ProblemDetailsFactory problemDetailsFactory,
        Func<Machine, IActionResult> successAction)
    {
        if (machine is null)
            return problemDetailsFactory.CreateProblemDetails(
                controller,
                ToStatusCodeFromMachineryError(MachineryError.MachineNotFound),
                MachineryError.MachineNotFound,
                errorLocalizer[nameof(MachineryError.MachineNotFound)]
            );
        return successAction(machine);
    }
}
