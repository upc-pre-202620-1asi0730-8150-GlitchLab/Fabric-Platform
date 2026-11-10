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
[Route("api/v1/machines/{machineId:int}/maintenances")]
[Produces(MediaTypeNames.Application.Json)]
[Tags("Available Machine Maintenance endpoints")]
public class MachineMaintenancesController(
    IMachineCommandService machineCommandService,
    IMachineQueryService machineQueryService,
    IStringLocalizer<ErrorMessages> errorLocalizer,
    ProblemDetailsFactory problemDetailsFactory)
    : ControllerBase
{
    private readonly IStringLocalizer<ErrorMessages> _errorLocalizer = errorLocalizer;
    private readonly ProblemDetailsFactory _problemDetailsFactory = problemDetailsFactory;

    [HttpPost]
    [EndpointSummary("Register a maintenance")]
    [EndpointDescription("Registers a maintenance performed on a machine")]
    [ProducesResponseType(typeof(MaintenanceRecordResource), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RegisterMaintenanceRecord([FromRoute] int machineId,
        [FromBody] RegisterMaintenanceRecordResource resource, CancellationToken cancellationToken)
    {
        var registerMaintenanceRecordCommand =
            RegisterMaintenanceRecordCommandFromResourceAssembler.ToCommandFromResource(resource, machineId);
        var result = await machineCommandService.Handle(registerMaintenanceRecordCommand, cancellationToken);

        return MachineryActionResultAssembler.ToActionResultFromRegisterMaintenanceRecordResult(
            this,
            result,
            _errorLocalizer,
            _problemDetailsFactory,
            maintenance => CreatedAtAction(nameof(GetMaintenanceRecords), new { machineId },
                MaintenanceRecordResourceFromEntityAssembler.ToResourceFromEntity(maintenance))
        );
    }

    [HttpGet]
    [EndpointSummary("Get the maintenance history of a machine")]
    [EndpointDescription("Get the maintenance records of a machine ordered chronologically")]
    [ProducesResponseType(typeof(IEnumerable<MaintenanceRecordResource>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetMaintenanceRecords([FromRoute] int machineId,
        CancellationToken cancellationToken)
    {
        var getMaintenanceRecordsQuery = new GetMaintenanceRecordsByMachineIdQuery(machineId);
        var maintenances = await machineQueryService.Handle(getMaintenanceRecordsQuery, cancellationToken);
        var maintenanceResources =
            maintenances.Select(MaintenanceRecordResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(maintenanceResources);
    }
}
