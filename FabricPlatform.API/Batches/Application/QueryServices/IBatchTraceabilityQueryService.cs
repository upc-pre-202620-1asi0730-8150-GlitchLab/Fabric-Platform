using FabricPlatform.API.Batches.Domain.Model.Entities;

namespace FabricPlatform.API.Batches.Application.QueryServices;

public interface IBatchTraceabilityQueryService
{
    Task<IEnumerable<BatchMovement>> HandleAsync(int batchId, CancellationToken cancellationToken = default);
}