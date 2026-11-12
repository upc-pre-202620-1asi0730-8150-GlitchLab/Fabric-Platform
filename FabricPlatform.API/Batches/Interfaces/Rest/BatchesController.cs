using System.Net.Mime;
using FabricPlatform.API.Batches.Application.CommandServices;
using FabricPlatform.API.Batches.Application.QueryServices;
using FabricPlatform.API.Batches.Domain.Model.Commands;
using FabricPlatform.API.Batches.Domain.Model.Errors;
using FabricPlatform.API.Batches.Domain.Model.Queries;
using FabricPlatform.API.Batches.Interfaces.Rest.Resources;
using FabricPlatform.API.Batches.Interfaces.Rest.Transform;
using FabricPlatform.API.Shared.Application.Model;
using FabricPlatform.API.Shared.Domain.Model.Errors;
using Microsoft.AspNetCore.Mvc;

namespace FabricPlatform.API.Batches.Interfaces.Rest;

[ApiController]
[Route("api/v1")]
[Produces(MediaTypeNames.Application.Json)]
[Tags("Available Batch endpoints")]
public class BatchesController(
    IBatchQueryService batchQueryService,
    IBatchTraceabilityQueryService batchTraceabilityQueryService,
    ICreateBatchCommandService createBatchCommandService,
    IAssignOperatorToBatchCommandService assignOperatorToBatchCommandService,
    IUpdateBatchStageCommandService updateBatchStageCommandService,
    IBatchOperationsCommandService batchOperationsCommandService,
    IProductionOperatorQueryService productionOperatorQueryService,
    ICreateProductionOperatorCommandService createProductionOperatorCommandService)
    : ControllerBase
{
    [HttpGet("batches")]
    [EndpointSummary("Get all batches")]
    [EndpointDescription("Gets all the production batches")]
    [ProducesResponseType(typeof(IEnumerable<BatchResource>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllBatches(CancellationToken cancellationToken)
    {
        var batches = await batchQueryService.HandleAsync(new GetAllBatchesQuery(), cancellationToken);
        return Ok(batches.Select(BatchResourceFromEntityAssembler.ToResourceFromEntity));
    }

    [HttpGet("batches/{id:int}")]
    [EndpointSummary("Get batch by ID")]
    [EndpointDescription("Gets a production batch by its unique identifier")]
    [ProducesResponseType(typeof(BatchResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetBatchById(int id, CancellationToken cancellationToken)
    {
        if (id <= 0) return BadRequest(BatchErrors.InvalidRequest);

        var batch = await batchQueryService.HandleAsync(new GetBatchByIdQuery(id), cancellationToken);
        return batch == null ? NotFound() : Ok(BatchResourceFromEntityAssembler.ToResourceFromEntity(batch));
    }

    [HttpPost("batches")]
    [EndpointSummary("Create a batch")]
    [EndpointDescription("Creates a new production batch")]
    [ProducesResponseType(typeof(BatchResource), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> CreateBatch([FromBody] CreateBatchResource resource,
        CancellationToken cancellationToken)
    {
        var result =
            await createBatchCommandService.HandleAsync(
                CreateBatchCommandFromResourceAssembler.ToCommandFromResource(resource), cancellationToken);
        if (result.IsFailure) return Failure(result);

        var batch = await batchQueryService.HandleAsync(new GetBatchByIdQuery(result.Value!), cancellationToken);
        if (batch == null) return NotFound();

        return CreatedAtAction(nameof(GetBatchById), new { id = batch.Id.Value },
            BatchResourceFromEntityAssembler.ToResourceFromEntity(batch));
    }

    [HttpPut("batches/{id:int}")]
    [EndpointSummary("Update the stage of a batch")]
    [EndpointDescription("Updates the stage, progress and status of a production batch")]
    [ProducesResponseType(typeof(BatchResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> UpdateBatch(int id, [FromBody] UpdateBatchStageResource resource,
        CancellationToken cancellationToken)
    {
        if (id <= 0) return BadRequest(BatchErrors.InvalidRequest);

        var command = UpdateBatchStageCommandFromResourceAssembler.ToCommandFromResource(id, resource);
        var result = await updateBatchStageCommandService.HandleAsync(command, cancellationToken);
        if (result.IsFailure) return Failure(result);

        var batch = await batchQueryService.HandleAsync(new GetBatchByIdQuery(id), cancellationToken);
        return batch == null ? NotFound() : Ok(BatchResourceFromEntityAssembler.ToResourceFromEntity(batch));
    }

    [HttpGet("traceabilityHistory/{id:int}")]
    [EndpointSummary("Get the traceability history of a batch")]
    [EndpointDescription("Gets the movements registered for a batch by stage")]
    [ProducesResponseType(typeof(IEnumerable<TraceabilityMovementResource>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetTraceabilityHistory(int id, CancellationToken cancellationToken)
    {
        if (id <= 0) return BadRequest(BatchErrors.InvalidRequest);

        var batch = await batchQueryService.HandleAsync(new GetBatchByIdQuery(id), cancellationToken);
        if (batch == null) return NotFound();

        var movements = await batchTraceabilityQueryService.HandleAsync(id, cancellationToken);
        return Ok(movements.Select(TraceabilityMovementResourceFromEntityAssembler.ToResourceFromEntity));
    }

    [HttpGet("traceabilitySummary/{id:int}")]
    [EndpointSummary("Get the traceability summary of a batch")]
    [EndpointDescription("Gets the traceability summary of a batch")]
    [ProducesResponseType(typeof(TraceabilitySummaryResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetTraceabilitySummary(int id, CancellationToken cancellationToken)
    {
        if (id <= 0) return BadRequest(BatchErrors.InvalidRequest);

        var batch = await batchQueryService.HandleAsync(new GetBatchByIdQuery(id), cancellationToken);
        return batch == null
            ? NotFound()
            : Ok(TraceabilitySummaryResourceFromEntityAssembler.ToResourceFromEntity(
                batch.GetTraceabilitySummary()));
    }

    [HttpPut("traceabilityHistory/{id:int}")]
    [EndpointSummary("Update a traceability movement")]
    [EndpointDescription("Updates a movement registered in the traceability history of a batch")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> UpdateTraceabilityMovement(int id,
        [FromBody] UpdateTraceabilityMovementResource resource, CancellationToken cancellationToken)
    {
        var command = new UpdateBatchMovementCommand(
            id,
            resource.Stage,
            resource.DateTime,
            resource.QtyIn,
            resource.QtyOut,
            resource.Responsible,
            resource.Status,
            resource.Difference);

        var result = await batchOperationsCommandService.HandleAsync(command, cancellationToken);
        return result.IsFailure ? Failure(result) : Ok(result.Value);
    }

    [HttpGet("operators")]
    [EndpointSummary("Get all production operators")]
    [EndpointDescription("Gets all the production operators")]
    [ProducesResponseType(typeof(IEnumerable<OperatorResource>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetOperators(CancellationToken cancellationToken)
    {
        var operators = await productionOperatorQueryService.HandleAsync(cancellationToken);
        return Ok(operators.Select(OperatorResourceFromEntityAssembler.ToResourceFromEntity));
    }

    [HttpPost("operators")]
    [EndpointSummary("Create a production operator")]
    [EndpointDescription("Creates a new production operator")]
    [ProducesResponseType(typeof(OperatorResource), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> CreateOperator(
        [FromBody] CreateProductionOperatorResource resource,
        CancellationToken cancellationToken)
    {
        var command = CreateProductionOperatorCommandFromResourceAssembler.ToCommandFromResource(resource);
        var result = await createProductionOperatorCommandService.HandleAsync(command, cancellationToken);
        return result.IsFailure
            ? Failure(result)
            : StatusCode(StatusCodes.Status201Created,
                OperatorResourceFromEntityAssembler.ToResourceFromEntity(result.Value!));
    }

    [HttpGet("assignedOperators/{id:int}")]
    [EndpointSummary("Get the operators assigned to a batch")]
    [EndpointDescription("Gets the operators assigned to a batch ordered by assignment date")]
    [ProducesResponseType(typeof(IEnumerable<AssignedOperatorResource>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAssignedOperators(int id, CancellationToken cancellationToken)
    {
        if (id <= 0) return BadRequest(BatchErrors.InvalidRequest);

        var batch = await batchQueryService.HandleAsync(new GetBatchByIdQuery(id), cancellationToken);
        return batch == null
            ? NotFound()
            : Ok(batch.Operators
                .OrderBy(assignedOperator => assignedOperator.AssignmentDate)
                .ThenBy(assignedOperator => assignedOperator.Id)
                .Select(AssignedOperatorResourceFromEntityAssembler.ToResourceFromEntity));
    }

    [HttpPost("assignedOperators")]
    [EndpointSummary("Assign an operator to a batch")]
    [EndpointDescription("Assigns a production operator to a stage of a batch")]
    [ProducesResponseType(typeof(AssignedOperatorResource), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> AssignOperator([FromBody] AssignOperatorResource resource,
        CancellationToken cancellationToken)
    {
        var command = AssignOperatorCommandFromResourceAssembler.ToCommandFromResource(resource.BatchId, resource);
        var result = await assignOperatorToBatchCommandService.HandleAsync(command, cancellationToken);
        if (result.IsFailure) return Failure(result);

        var batch = await batchQueryService.HandleAsync(new GetBatchByIdQuery(resource.BatchId), cancellationToken);
        var assignedOperator = batch?.Operators.FirstOrDefault(item => item.Id == result.Value);
        return assignedOperator == null
            ? StatusCode(StatusCodes.Status201Created, result.Value)
            : StatusCode(StatusCodes.Status201Created,
                AssignedOperatorResourceFromEntityAssembler.ToResourceFromEntity(assignedOperator));
    }

    [HttpGet("observations/{id:int}")]
    [EndpointSummary("Get the observations of a batch")]
    [EndpointDescription("Gets the observations registered for a batch in chronological order")]
    [ProducesResponseType(typeof(IEnumerable<ObservationResource>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetObservations(int id, CancellationToken cancellationToken)
    {
        if (id <= 0) return BadRequest(BatchErrors.InvalidRequest);

        var batch = await batchQueryService.HandleAsync(new GetBatchByIdQuery(id), cancellationToken);
        return batch == null
            ? NotFound()
            : Ok(batch.Observations
                .OrderBy(observation => observation.CreatedAt)
                .ThenBy(observation => observation.Id)
                .Select(ObservationResourceFromEntityAssembler.ToResourceFromEntity));
    }

    [HttpPost("observations")]
    [EndpointSummary("Create a batch observation")]
    [EndpointDescription("Registers a new observation for a batch")]
    [ProducesResponseType(typeof(ObservationResource), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CreateObservation([FromBody] CreateObservationResource resource,
        CancellationToken cancellationToken)
    {
        var command = CreateBatchObservationCommandFromResourceAssembler.ToCommandFromResource(resource);
        var result = await batchOperationsCommandService.HandleAsync(command, cancellationToken);
        if (result.IsFailure) return Failure(result);

        var batch = await batchQueryService.HandleAsync(new GetBatchByIdQuery(resource.BatchId), cancellationToken);
        var observation = batch?.Observations.FirstOrDefault(item => item.Id == result.Value);
        return observation == null
            ? StatusCode(StatusCodes.Status201Created, result.Value)
            : StatusCode(StatusCodes.Status201Created,
                ObservationResourceFromEntityAssembler.ToResourceFromEntity(observation));
    }

    private IActionResult Failure<T>(Result<T> result)
    {
        return result.DomainError.Type switch
        {
            EErrorType.NotFound => NotFound(result.DomainError),
            EErrorType.Conflict => Conflict(result.DomainError),
            EErrorType.Validation => BadRequest(result.DomainError),
            _ => BadRequest(result.DomainError)
        };
    }
}