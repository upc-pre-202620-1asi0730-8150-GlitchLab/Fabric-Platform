using FabricPlatform.API.Batches.Domain.Model.Commands;
using FabricPlatform.API.Shared.Application.Model;

namespace FabricPlatform.API.Batches.Application.CommandServices;

public interface IAssignOperatorToBatchCommandService
{
    Task<Result<int>> HandleAsync(AssignOperatorToBatchCommand command, CancellationToken cancellationToken = default);
}