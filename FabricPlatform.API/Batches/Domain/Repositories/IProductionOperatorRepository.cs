using FabricPlatform.API.Batches.Domain.Model.Aggregates;

namespace FabricPlatform.API.Batches.Domain.Repositories;

public interface IProductionOperatorRepository
{
    Task<IReadOnlyList<ProductionOperator>> ListAsync(CancellationToken cancellationToken = default);
    Task<ProductionOperator?> FindByNameAsync(string name, CancellationToken cancellationToken = default);
    Task AddAsync(ProductionOperator productionOperator, CancellationToken cancellationToken = default);
}
