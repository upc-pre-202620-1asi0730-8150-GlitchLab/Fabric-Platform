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
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Tags("Available Machine endpoints")]
public class MachinesController(
    IMachineCommandService machineCommandService,
    IMachineQueryService machineQueryService,
    IStringLocalizer<ErrorMessages> errorLocalizer,
    ProblemDetailsFactory problemDetailsFactory)
    : ControllerBase
{
    private readonly IStringLocalizer<ErrorMessages> _errorLocalizer = errorLocalizer;
    private readonly ProblemDetailsFactory _problemDetailsFactory = problemDetailsFactory;

    [HttpPost]
    [EndpointSummary("Register a machine")]
    [EndpointDescription("Registers a new machine of the workshop with Operational status")]
    [ProducesResponseType(typeof(MachineResource), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> RegisterMachine([FromBody] RegisterMachineResource resource,
        CancellationToken cancellationToken)
    {
        var registerMachineCommand = RegisterMachineCommandFromResourceAssembler.ToCommandFromResource(resource);
        var result = await machineCommandService.Handle(registerMachineCommand, cancellationToken);

        return MachineryActionResultAssembler.ToActionResultFromRegisterMachineResult(
            this,
            result,
            _errorLocalizer,
            _problemDetailsFactory,
            createdMachine => CreatedAtAction(nameof(GetMachineById), new { machineId = createdMachine.Id.Value },
                MachineResourceFromEntityAssembler.ToResourceFromEntity(createdMachine))
        );
    }

    [HttpGet("{machineId:int}")]
    [EndpointSummary("Get a machine by its id")]
    [EndpointDescription("Get a machine by its id, including its current status and accumulated downtime")]
    [ProducesResponseType(typeof(MachineResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetMachineById([FromRoute] int machineId, CancellationToken cancellationToken)
    {
        var getMachineByIdQuery = new GetMachineByIdQuery(machineId);
        var machine = await machineQueryService.Handle(getMachineByIdQuery, cancellationToken);

        return MachineryActionResultAssembler.ToActionResultFromGetMachineByIdResult(
            this,
            machine,
            _errorLocalizer,
            _problemDetailsFactory,
            foundMachine => Ok(MachineResourceFromEntityAssembler.ToResourceFromEntity(foundMachine))
        );
    }

    [HttpGet]
    [EndpointSummary("Get all machines")]
    [EndpointDescription("Get all machines with their current status. Optionally filter by machine code")]
    [ProducesResponseType(typeof(IEnumerable<MachineResource>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllMachines([FromQuery] string? code, CancellationToken cancellationToken)
    {
        if (!string.IsNullOrWhiteSpace(code))
        {
            var getMachineByCodeQuery = new GetMachineByCodeQuery(code);
            var machine = await machineQueryService.Handle(getMachineByCodeQuery, cancellationToken);
            var filteredResources = machine is null
                ? []
                : new[] { MachineResourceFromEntityAssembler.ToResourceFromEntity(machine) };
            return Ok(filteredResources);
        }

        var getAllMachinesQuery = new GetAllMachinesQuery();
        var machines = await machineQueryService.Handle(getAllMachinesQuery, cancellationToken);
        var machineResources = machines.Select(MachineResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(machineResources);
    }

    [HttpPut("{machineId:int}/status")]
    [EndpointSummary("Update the status of a machine")]
    [EndpointDescription("Updates the operational status of a machine (Operational, InMaintenance, Stopped)")]
    [ProducesResponseType(typeof(MachineResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateMachineStatus([FromRoute] int machineId,
        [FromBody] UpdateMachineStatusResource resource, CancellationToken cancellationToken)
    {
        var updateMachineStatusCommand =
            UpdateMachineStatusCommandFromResourceAssembler.ToCommandFromResource(resource, machineId);
        var result = await machineCommandService.Handle(updateMachineStatusCommand, cancellationToken);

        return MachineryActionResultAssembler.ToActionResultFromUpdateMachineStatusResult(
            this,
            result,
            _errorLocalizer,
            _problemDetailsFactory,
            updatedMachine => Ok(MachineResourceFromEntityAssembler.ToResourceFromEntity(updatedMachine))
        );
    }
}
