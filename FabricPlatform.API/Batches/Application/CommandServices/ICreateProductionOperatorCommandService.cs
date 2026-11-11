using FabricPlatform.API.Batches.Domain.Model.Aggregates;
using FabricPlatform.API.Batches.Domain.Model.Commands;
using FabricPlatform.API.Shared.Application.Model;

namespace FabricPlatform.API.Batches.Application.CommandServices;

public interface ICreateProductionOperatorCommandService
{
    Task<Result<ProductionOperator>> HandleAsync(
        CreateProductionOperatorCommand command,
        CancellationToken cancellationToken = default);
}
