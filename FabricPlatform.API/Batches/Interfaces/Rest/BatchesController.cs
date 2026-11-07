using System.Net.Mime;
using FabricPlatform.API.Batches.Application.QueryServices;
using FabricPlatform.API.Batches.Domain.Model.Queries;
using FabricPlatform.API.Batches.Interfaces.Rest.Transform;
using Microsoft.AspNetCore.Mvc;

namespace FabricPlatform.API.Batches.Interfaces.Rest;

[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
public class BatchesController : ControllerBase
{
    private readonly IBatchQueryService _batchQueryService;
    private readonly IBatchTraceabilityQueryService _batchTraceabilityQueryService;
    // The corresponding Command Services (Create, AssignOperator, UpdateStage) would also be injected here, 
    // following the command facade pattern used in your architecture.

    public BatchesController(
        IBatchQueryService batchQueryService,
        IBatchTraceabilityQueryService batchTraceabilityQueryService)
    {
        _batchQueryService = batchQueryService;
        _batchTraceabilityQueryService = batchTraceabilityQueryService;
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetBatchById(int id)
    {
        var query = new GetBatchByIdQuery(id);
        var batch = await _batchQueryService.HandleAsync(query);

        if (batch == null) return NotFound();

        var resource = BatchResourceFromEntityAssembler.ToResourceFromEntity(batch);
        return Ok(resource);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllBatches()
    {
        var query = new GetAllBatchesQuery();
        var batches = await _batchQueryService.HandleAsync(query);

        var resources = batches.Select(BatchResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(resources);
    }

    [HttpGet("{id:int}/movements")]
    public async Task<IActionResult> GetBatchMovements(int id)
    {
        var movements = await _batchTraceabilityQueryService.HandleAsync(id);
        return Ok(movements);
    }
}