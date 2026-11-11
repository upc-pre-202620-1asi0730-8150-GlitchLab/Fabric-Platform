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
public class BatchesController : ControllerBase
{
    private readonly IBatchQueryService _batchQueryService;
    private readonly IBatchTraceabilityQueryService _batchTraceabilityQueryService;
    private readonly ICreateBatchCommandService _createBatchCommandService;
    private readonly IAssignOperatorToBatchCommandService _assignOperatorToBatchCommandService;
    private readonly IUpdateBatchStageCommandService _updateBatchStageCommandService;
    private readonly IBatchOperationsCommandService _batchOperationsCommandService;
    private readonly IProductionOperatorQueryService _productionOperatorQueryService;
    private readonly ICreateProductionOperatorCommandService _createProductionOperatorCommandService;

    public BatchesController(
        IBatchQueryService batchQueryService,
        IBatchTraceabilityQueryService batchTraceabilityQueryService,
        ICreateBatchCommandService createBatchCommandService,
        IAssignOperatorToBatchCommandService assignOperatorToBatchCommandService,
        IUpdateBatchStageCommandService updateBatchStageCommandService,
        IBatchOperationsCommandService batchOperationsCommandService,
        IProductionOperatorQueryService productionOperatorQueryService,
        ICreateProductionOperatorCommandService createProductionOperatorCommandService)
    {
        _batchQueryService = batchQueryService;
        _batchTraceabilityQueryService = batchTraceabilityQueryService;
        _createBatchCommandService = createBatchCommandService;
        _assignOperatorToBatchCommandService = assignOperatorToBatchCommandService;
        _updateBatchStageCommandService = updateBatchStageCommandService;
        _batchOperationsCommandService = batchOperationsCommandService;
        _productionOperatorQueryService = productionOperatorQueryService;
        _createProductionOperatorCommandService = createProductionOperatorCommandService;
    }

    [HttpGet("batches")]
    public async Task<IActionResult> GetAllBatches(CancellationToken cancellationToken)
    {
        var batches = await _batchQueryService.HandleAsync(new GetAllBatchesQuery(), cancellationToken);
        return Ok(batches.Select(BatchResourceFromEntityAssembler.ToResourceFromEntity));
    }

    [HttpGet("batches/{id:int}")]
    public async Task<IActionResult> GetBatchById(int id, CancellationToken cancellationToken)
    {
        if (id <= 0) return BadRequest(BatchErrors.InvalidRequest);

        var batch = await _batchQueryService.HandleAsync(new GetBatchByIdQuery(id), cancellationToken);
        return batch == null ? NotFound() : Ok(BatchResourceFromEntityAssembler.ToResourceFromEntity(batch));
    }

    [HttpPost("batches")]
    public async Task<IActionResult> CreateBatch([FromBody] CreateBatchResource resource,
        CancellationToken cancellationToken)
    {
        var result =
            await _createBatchCommandService.HandleAsync(
                CreateBatchCommandFromResourceAssembler.ToCommandFromResource(resource), cancellationToken);
        if (result.IsFailure) return Failure(result);

        var batch = await _batchQueryService.HandleAsync(new GetBatchByIdQuery(result.Value!), cancellationToken);
        if (batch == null) return NotFound();

        return CreatedAtAction(nameof(GetBatchById), new { id = batch.Id.Value },
            BatchResourceFromEntityAssembler.ToResourceFromEntity(batch));
    }

    [HttpPatch("batches/{id:int}")]
    public async Task<IActionResult> PatchBatch(int id, [FromBody] UpdateBatchStageResource resource,
        CancellationToken cancellationToken)
    {
        if (id <= 0) return BadRequest(BatchErrors.InvalidRequest);

        var command = UpdateBatchStageCommandFromResourceAssembler.ToCommandFromResource(id, resource);
        var result = await _updateBatchStageCommandService.HandleAsync(command, cancellationToken);
        if (result.IsFailure) return Failure(result);

        var batch = await _batchQueryService.HandleAsync(new GetBatchByIdQuery(id), cancellationToken);
        return batch == null ? NotFound() : Ok(BatchResourceFromEntityAssembler.ToResourceFromEntity(batch));
    }

    [HttpGet("traceabilityHistory/{id:int}")]
    public async Task<IActionResult> GetTraceabilityHistory(int id, CancellationToken cancellationToken)
    {
        if (id <= 0) return BadRequest(BatchErrors.InvalidRequest);

        var batch = await _batchQueryService.HandleAsync(new GetBatchByIdQuery(id), cancellationToken);
        if (batch == null) return NotFound();

        var movements = await _batchTraceabilityQueryService.HandleAsync(id, cancellationToken);
        return Ok(movements.Select(TraceabilityMovementResourceFromEntityAssembler.ToResourceFromEntity));
    }

    [HttpGet("traceabilitySummary/{id:int}")]
    public async Task<IActionResult> GetTraceabilitySummary(int id, CancellationToken cancellationToken)
    {
        if (id <= 0) return BadRequest(BatchErrors.InvalidRequest);

        var batch = await _batchQueryService.HandleAsync(new GetBatchByIdQuery(id), cancellationToken);
        return batch == null
            ? NotFound()
            : Ok(TraceabilitySummaryResourceFromEntityAssembler.ToResourceFromEntity(
                batch.GetTraceabilitySummary()));
    }

    [HttpPatch("traceabilityHistory/{id:int}")]
    public async Task<IActionResult> PatchTraceabilityMovement(int id,
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

        var result = await _batchOperationsCommandService.HandleAsync(command, cancellationToken);
        return result.IsFailure ? Failure(result) : Ok(result.Value);
    }

    [HttpGet("operators")]
    public async Task<IActionResult> GetOperators(CancellationToken cancellationToken)
    {
        var operators = await _productionOperatorQueryService.HandleAsync(cancellationToken);
        return Ok(operators.Select(OperatorResourceFromEntityAssembler.ToResourceFromEntity));
    }

    [HttpPost("operators")]
    public async Task<IActionResult> CreateOperator(
        [FromBody] CreateProductionOperatorResource resource,
        CancellationToken cancellationToken)
    {
        var command = CreateProductionOperatorCommandFromResourceAssembler.ToCommandFromResource(resource);
        var result = await _createProductionOperatorCommandService.HandleAsync(command, cancellationToken);
        return result.IsFailure
            ? Failure(result)
            : StatusCode(StatusCodes.Status201Created,
                OperatorResourceFromEntityAssembler.ToResourceFromEntity(result.Value!));
    }

    [HttpGet("assignedOperators/{id:int}")]
    public async Task<IActionResult> GetAssignedOperators(int id, CancellationToken cancellationToken)
    {
        if (id <= 0) return BadRequest(BatchErrors.InvalidRequest);

        var batch = await _batchQueryService.HandleAsync(new GetBatchByIdQuery(id), cancellationToken);
        return batch == null
            ? NotFound()
            : Ok(batch.Operators
                .OrderBy(assignedOperator => assignedOperator.AssignmentDate)
                .ThenBy(assignedOperator => assignedOperator.Id)
                .Select(AssignedOperatorResourceFromEntityAssembler.ToResourceFromEntity));
    }

    [HttpPost("assignedOperators")]
    public async Task<IActionResult> AssignOperator([FromBody] AssignOperatorResource resource,
        CancellationToken cancellationToken)
    {
        var command = AssignOperatorCommandFromResourceAssembler.ToCommandFromResource(resource.BatchId, resource);
        var result = await _assignOperatorToBatchCommandService.HandleAsync(command, cancellationToken);
        if (result.IsFailure) return Failure(result);

        var batch = await _batchQueryService.HandleAsync(new GetBatchByIdQuery(resource.BatchId), cancellationToken);
        var assignedOperator = batch?.Operators.FirstOrDefault(item => item.Id == result.Value);
        return assignedOperator == null
            ? StatusCode(StatusCodes.Status201Created, result.Value)
            : StatusCode(StatusCodes.Status201Created,
                AssignedOperatorResourceFromEntityAssembler.ToResourceFromEntity(assignedOperator));
    }

    [HttpGet("observations/{id:int}")]
    public async Task<IActionResult> GetObservations(int id, CancellationToken cancellationToken)
    {
        if (id <= 0) return BadRequest(BatchErrors.InvalidRequest);

        var batch = await _batchQueryService.HandleAsync(new GetBatchByIdQuery(id), cancellationToken);
        return batch == null
            ? NotFound()
            : Ok(batch.Observations
                .OrderBy(observation => observation.CreatedAt)
                .ThenBy(observation => observation.Id)
                .Select(ObservationResourceFromEntityAssembler.ToResourceFromEntity));
    }

    [HttpPost("observations")]
    public async Task<IActionResult> CreateObservation([FromBody] CreateObservationResource resource,
        CancellationToken cancellationToken)
    {
        var command = CreateBatchObservationCommandFromResourceAssembler.ToCommandFromResource(resource);
        var result = await _batchOperationsCommandService.HandleAsync(command, cancellationToken);
        if (result.IsFailure) return Failure(result);

        var batch = await _batchQueryService.HandleAsync(new GetBatchByIdQuery(resource.BatchId), cancellationToken);
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