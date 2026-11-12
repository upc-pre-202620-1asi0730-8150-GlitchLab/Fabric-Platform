namespace FabricPlatform.API.Quality.Application.Internal.CommandHandlers;

using System.Threading;
using System.Threading.Tasks;
using FabricPlatform.API.Quality.Application.Internal.Commands;
using FabricPlatform.API.Quality.Domain.Model.Aggregates;
using FabricPlatform.API.Quality.Domain.Repositories;
using FabricPlatform.API.Shared.Application.Model;
using FabricPlatform.API.Shared.Domain.Model.Errors;
using FabricPlatform.API.Shared.Domain.Repositories;

public class UpdateDefectCommandHandler(
    IDefectRepository defectRepository,
    IUnitOfWork unitOfWork)
{
    public async Task<Result<Defect>> Handle(UpdateDefectCommand command, CancellationToken ct = default)
    {
        var defect = await defectRepository.FindByIdAsync(command.DefectId, ct);
        if (defect is null)
        {
            return Result<Defect>.Failure(
                Error.NotFound("Defect.NotFound", $"Defect with ID {command.DefectId} was not found."));
        }

        defect.Update(command.Status, command.Observation, command.Disposition, command.CorrectionType);
        defectRepository.Update(defect);
        await unitOfWork.CompleteAsync(ct);

        return Result<Defect>.Success(defect);
    }
}