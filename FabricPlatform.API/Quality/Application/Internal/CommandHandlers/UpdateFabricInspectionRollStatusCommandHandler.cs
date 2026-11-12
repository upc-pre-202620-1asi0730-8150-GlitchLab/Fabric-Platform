namespace FabricPlatform.API.Quality.Application.Internal.CommandHandlers;

using System.Threading;
using System.Threading.Tasks;
using FabricPlatform.API.Quality.Application.Internal.Commands;
using FabricPlatform.API.Quality.Domain.Model.Aggregates;
using FabricPlatform.API.Quality.Domain.Repositories;
using FabricPlatform.API.Shared.Application.Model;
using FabricPlatform.API.Shared.Domain.Model.Errors;
using FabricPlatform.API.Shared.Domain.Repositories;

public class UpdateFabricInspectionRollStatusCommandHandler(
    IFabricInspectionRepository inspectionRepository,
    IUnitOfWork unitOfWork)
{
    public async Task<Result<FabricInspection>> Handle(UpdateFabricInspectionRollStatusCommand command, CancellationToken ct = default)
    {
        var inspection = await inspectionRepository.FindByIdAsync(command.InspectionId, ct);
        if (inspection is null)
        {
            return Result<FabricInspection>.Failure(
                Error.NotFound("Inspection.NotFound", $"Fabric inspection with ID {command.InspectionId} was not found."));
        }

        if (!FabricInspection.RollStatuses.Contains(command.RollStatus))
        {
            return Result<FabricInspection>.Failure(
                Error.Validation("Inspection.InvalidRollStatus", $"Roll status '{command.RollStatus}' is not valid."));
        }

        inspection.UpdateRollStatus(command.RollStatus);

        inspectionRepository.Update(inspection);
        await unitOfWork.CompleteAsync(ct);

        return Result<FabricInspection>.Success(inspection);
    }
}
