using FabricPlatform.API.Shared.Domain.Repositories;
using FabricPlatform.API.Batches.Domain.Model.Aggregates;
using FabricPlatform.API.Batches.Domain.Model.ValueObjects;

namespace FabricPlatform.API.Batches.Domain.Repositories;

public interface IBatchRepository: IBaseRepository<Batch>
{
    Task<Batch?> FindByIdAsync(BatchId id, CancellationToken cancellationToken = default);
    Task<Batch?> FindByCodeAsync(BatchCode code, CancellationToken cancellationToken = default);
    Task<Batch?> FindByMovementIdAsync(int movementId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Batch>> ListByStatusAsync(BatchStatus status, CancellationToken cancellationToken = default);
}