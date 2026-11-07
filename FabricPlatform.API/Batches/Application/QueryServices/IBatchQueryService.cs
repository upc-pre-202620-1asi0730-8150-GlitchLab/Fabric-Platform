using FabricPlatform.API.Batches.Domain.Model.Queries;
using FabricPlatform.API.Batches.Domain.Model.Aggregates;

namespace FabricPlatform.API.Batches.Application.QueryServices;

public interface IBatchQueryService
{
    Task<Batch?> HandleAsync(GetBatchByIdQuery query, CancellationToken cancellationToken = default);
    Task<IEnumerable<Batch>> HandleAsync(GetAllBatchesQuery query, CancellationToken cancellationToken = default);
}