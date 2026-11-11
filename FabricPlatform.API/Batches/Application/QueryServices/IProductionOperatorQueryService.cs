using FabricPlatform.API.Batches.Domain.Model.Aggregates;

namespace FabricPlatform.API.Batches.Application.QueryServices;

public interface IProductionOperatorQueryService
{
    Task<IReadOnlyList<ProductionOperator>> HandleAsync(CancellationToken cancellationToken = default);
}
