using FabricPlatform.API.Batches.Application.QueryServices;
using FabricPlatform.API.Batches.Domain.Model.Aggregates;
using FabricPlatform.API.Batches.Domain.Repositories;

namespace FabricPlatform.API.Batches.Application.Internal.QueryServices;

public class ProductionOperatorQueryService : IProductionOperatorQueryService
{
    private readonly IProductionOperatorRepository _operatorRepository;

    public ProductionOperatorQueryService(IProductionOperatorRepository operatorRepository)
    {
        _operatorRepository = operatorRepository;
    }

    public Task<IReadOnlyList<ProductionOperator>> HandleAsync(CancellationToken cancellationToken = default)
    {
        return _operatorRepository.ListAsync(cancellationToken);
    }
}
