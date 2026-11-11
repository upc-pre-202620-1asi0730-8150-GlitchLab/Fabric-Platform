using FabricPlatform.API.Batches.Domain.Model.Commands;
using FabricPlatform.API.Shared.Application.Model;

namespace FabricPlatform.API.Batches.Application.CommandServices;

public interface IBatchOperationsCommandService
{
    Task<Result<bool>> HandleAsync(UpdateBatchMovementCommand command, CancellationToken cancellationToken = default);
    Task<Result<int>> HandleAsync(CreateBatchObservationCommand command, CancellationToken cancellationToken = default);
}
