namespace FabricPlatform.API.Quality.Application.Internal.CommandHandlers;

using System.Threading;
using System.Threading.Tasks;
using FabricPlatform.API.Quality.Application.Internal.Commands;
using FabricPlatform.API.Quality.Domain.Model.Aggregates;
using FabricPlatform.API.Quality.Domain.Repositories;
using FabricPlatform.API.Shared.Application.Model;
using FabricPlatform.API.Shared.Domain.Repositories;

public class CreateFabricInspectionCommandHandler(
    IFabricInspectionRepository inspectionRepository,
    IUnitOfWork unitOfWork)
{
    public async Task<Result<FabricInspection>> Handle(CreateFabricInspectionCommand command, CancellationToken ct = default)
    {
        var inspection = new FabricInspection(
            command.RollId,
            command.Supplier,
            command.InspectionDate,
            command.Sample,
            command.Color,
            command.InitialResult,
            command.RollStatus,
            command.RollLength,
            command.ShadeMatch,
            command.WeaveCondition,
            command.Observation);

        await inspectionRepository.AddAsync(inspection, ct);
        await unitOfWork.CompleteAsync(ct);

        return Result<FabricInspection>.Success(inspection);
    }
}