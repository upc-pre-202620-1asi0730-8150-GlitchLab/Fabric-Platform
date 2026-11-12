using FabricPlatform.API.Batches.Domain.Model.Commands;
using FabricPlatform.API.Shared.Application.Model;

namespace FabricPlatform.API.Batches.Application.CommandServices;

public interface ICreateBatchCommandService
{
    Task<Result<int>> HandleAsync(CreateBatchCommand command, CancellationToken cancellationToken = default);
}