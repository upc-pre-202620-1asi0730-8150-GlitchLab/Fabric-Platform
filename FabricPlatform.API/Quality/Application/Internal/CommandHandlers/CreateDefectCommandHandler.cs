namespace FabricPlatform.API.Quality.Application.Internal.CommandHandlers;

using System.Threading;
using System.Threading.Tasks;
using FabricPlatform.API.Quality.Application.Internal.Commands;
using FabricPlatform.API.Quality.Domain.Model.Aggregates;
using FabricPlatform.API.Quality.Domain.Repositories;
using FabricPlatform.API.Shared.Application.Model;
using FabricPlatform.API.Shared.Domain.Repositories;

public class CreateDefectCommandHandler(
    IDefectRepository defectRepository,
    IUnitOfWork unitOfWork)
{
    public async Task<Result<Defect>> Handle(CreateDefectCommand command, CancellationToken ct = default)
    {
        var defect = new Defect(
            command.BatchId,
            command.DefectType,
            command.Quantity,
            command.MachineId,
            command.Origin,
            command.Status,
            command.Observation,
            command.Disposition,
            command.CorrectionType,
            command.Date);

        await defectRepository.AddAsync(defect, ct);
        await unitOfWork.CompleteAsync(ct);

        return Result<Defect>.Success(defect);
    }
}