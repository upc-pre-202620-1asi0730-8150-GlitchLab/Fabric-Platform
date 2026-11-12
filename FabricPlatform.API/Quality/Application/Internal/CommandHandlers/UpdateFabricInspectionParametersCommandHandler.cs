namespace FabricPlatform.API.Quality.Application.Internal.CommandHandlers;

using System.Threading;
using System.Threading.Tasks;
using FabricPlatform.API.Quality.Application.Internal.Commands;
using FabricPlatform.API.Quality.Domain.Model.Aggregates;
using FabricPlatform.API.Quality.Domain.Repositories;
using FabricPlatform.API.Shared.Application.Model;
using FabricPlatform.API.Shared.Domain.Model.Errors;
using FabricPlatform.API.Shared.Domain.Repositories;

public class UpdateFabricInspectionParametersCommandHandler(
    IFabricInspectionRepository inspectionRepository,
    IUnitOfWork unitOfWork)
{
    public async Task<Result<FabricInspection>> Handle(UpdateFabricInspectionParametersCommand command, CancellationToken ct = default)
    {
        var inspection = await inspectionRepository.FindByIdAsync(command.InspectionId, ct);
        if (inspection is null)
        {
            return Result<FabricInspection>.Failure(
                Error.NotFound("Inspection.NotFound", $"Fabric inspection with ID {command.InspectionId} was not found."));
        }

        if (command.FabricWidth.HasValue)
        {
            inspection.UpdateParameters(
                command.FabricWidth.Value,
                command.FabricWeight,
                command.FabricLength,
                command.ShadeCondition ?? "Matches approved sample",
                command.WeaveCondition ?? "No weave defects",
                command.SurfaceCondition ?? "No stains or damage",
                command.DefectsDetected ?? "None",
                command.Observations,
                command.ParameterHistory);
        }

        if (!string.IsNullOrWhiteSpace(command.ShrinkageTest) || command.TestHistory != null)
        {
            inspection.UpdateShrinkageTest(command.ShrinkageTest ?? "Approved", command.TestHistory);
        }

        inspectionRepository.Update(inspection);
        await unitOfWork.CompleteAsync(ct);

        return Result<FabricInspection>.Success(inspection);
    }
}