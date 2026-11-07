using FabricPlatform.API.Batches.Domain.Model.Aggregates;
using FabricPlatform.API.Batches.Domain.Model.Entities;
using FabricPlatform.API.Batches.Domain.Model.Queries;
using FabricPlatform.API.Batches.Domain.Repositories;
using FabricPlatform.API.Batches.Application.QueryServices;

namespace FabricPlatform.API.Batches.Application.Internal.QueryServices;

public class BatchQueryService : IBatchQueryService, IBatchTraceabilityQueryService
{
    private readonly IBatchRepository _batchRepository;

    public BatchQueryService(IBatchRepository batchRepository)
    {
        _batchRepository = batchRepository;
    }

    public async Task<Batch?> HandleAsync(GetBatchByIdQuery query, CancellationToken cancellationToken = default)
    {
        var batchId = new Domain.Model.ValueObjects.BatchId(query.Id);
        return await _batchRepository.FindByIdAsync(batchId, cancellationToken);
    }

    public async Task<IEnumerable<Batch>> HandleAsync(GetAllBatchesQuery query, CancellationToken cancellationToken = default)
    {
        return await _batchRepository.ListAsync(cancellationToken);
    }

    public async Task<IEnumerable<BatchMovement>> HandleAsync(int batchId, CancellationToken cancellationToken = default)
    {
        var id = new Domain.Model.ValueObjects.BatchId(batchId);
        var batch = await _batchRepository.FindByIdAsync(id, cancellationToken);
        
        return batch?.Movements ?? Enumerable.Empty<BatchMovement>();
    }
}