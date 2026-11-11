using System.Net.Mime;
using FabricPlatform.API.Machinery.Application.CommandServices;
using FabricPlatform.API.Machinery.Application.QueryServices;
using FabricPlatform.API.Machinery.Domain.Model.Queries;
using FabricPlatform.API.Machinery.Interfaces.Rest.Resources;
using FabricPlatform.API.Machinery.Interfaces.Rest.Transform;
using FabricPlatform.API.Shared.Interfaces.Rest.ProblemDetails;
using FabricPlatform.API.Shared.Resources.Errors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace FabricPlatform.API.Machinery.Interfaces.Rest;

[ApiController]
[Route("api/v1/machines/{machineId:int}/downtimes")]
[Produces(MediaTypeNames.Application.Json)]
[Tags("Available Machine Downtime endpoints")]
public class MachineDowntimesController(
    IMachineCommandService machineCommandService,
    IMachineQueryService machineQueryService,
    IStringLocalizer<ErrorMessages> errorLocalizer,
    ProblemDetailsFactory problemDetailsFactory)
    : ControllerBase
{
    private readonly IStringLocalizer<ErrorMessages> _errorLocalizer = errorLocalizer;
    private readonly ProblemDetailsFactory _problemDetailsFactory = problemDetailsFactory;

    [HttpPost]
    [EndpointSummary("Report a machine downtime")]
    [EndpointDescription("Registers a stop of the machine and changes its status to Stopped")]
    [ProducesResponseType(typeof(MachineDowntimeResource), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> ReportMachineDowntime([FromRoute] int machineId,
        [FromBody] ReportMachineDowntimeResource resource, CancellationToken cancellationToken)
    {
        var reportMachineDowntimeCommand =
            ReportMachineDowntimeCommandFromResourceAssembler.ToCommandFromResource(resource, machineId);
        var result = await machineCommandService.Handle(reportMachineDowntimeCommand, cancellationToken);

        return MachineryActionResultAssembler.ToActionResultFromMachineDowntimeResult(
            this,
            result,
            _errorLocalizer,
            _problemDetailsFactory,
            downtime => CreatedAtAction(nameof(GetMachineDowntimes), new { machineId },
                MachineDowntimeResourceFromEntityAssembler.ToResourceFromEntity(downtime))
        );
    }

    [HttpGet]
    [EndpointSummary("Get the downtimes of a machine")]
    [EndpointDescription("Get the downtimes of a machine ordered chronologically")]
    [ProducesResponseType(typeof(IEnumerable<MachineDowntimeResource>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetMachineDowntimes([FromRoute] int machineId,
        CancellationToken cancellationToken)
    {
        var getMachineDowntimesQuery = new GetMachineDowntimesByMachineIdQuery(machineId);
        var downtimes = await machineQueryService.Handle(getMachineDowntimesQuery, cancellationToken);
        var downtimeResources = downtimes.Select(MachineDowntimeResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(downtimeResources);
    }

    [HttpPatch("{downtimeId:int}/resumption")]
    [EndpointSummary("Resume the operation of a machine")]
    [EndpointDescription(
        "Closes an open downtime, calculates its duration in minutes and restores the machine status to Operational")]
    [ProducesResponseType(typeof(MachineDowntimeResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> ResumeMachineOperation([FromRoute] int machineId, [FromRoute] int downtimeId,
        [FromBody] ResumeMachineOperationResource resource, CancellationToken cancellationToken)
    {
        var resumeMachineOperationCommand =
            ResumeMachineOperationCommandFromResourceAssembler.ToCommandFromResource(resource, machineId, downtimeId);
        var result = await machineCommandService.Handle(resumeMachineOperationCommand, cancellationToken);

        return MachineryActionResultAssembler.ToActionResultFromMachineDowntimeResult(
            this,
            result,
            _errorLocalizer,
            _problemDetailsFactory,
            downtime => Ok(MachineDowntimeResourceFromEntityAssembler.ToResourceFromEntity(downtime))
        );
    }
}
