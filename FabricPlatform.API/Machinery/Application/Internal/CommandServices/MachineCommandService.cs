using FabricPlatform.API.Machinery.Application.CommandServices;
using FabricPlatform.API.Machinery.Domain.Model;
using FabricPlatform.API.Machinery.Domain.Model.Aggregates;
using FabricPlatform.API.Machinery.Domain.Model.Commands;
using FabricPlatform.API.Machinery.Domain.Model.Entities;
using FabricPlatform.API.Machinery.Domain.Model.Errors;
using FabricPlatform.API.Machinery.Domain.Model.ValueObjects;
using FabricPlatform.API.Machinery.Domain.Repositories;
using FabricPlatform.API.Shared.Application.Model;
using FabricPlatform.API.Shared.Domain.Repositories;

namespace FabricPlatform.API.Machinery.Application.Internal.CommandServices;

public class MachineCommandService(
    IMachineRepository machineRepository,
    IUnitOfWork unitOfWork)
    : IMachineCommandService
{
    public async Task<Result<Machine>> Handle(RegisterMachineCommand command, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(command.Code) || string.IsNullOrWhiteSpace(command.Type) ||
            string.IsNullOrWhiteSpace(command.Location) || command.AcquiredDate is null)
            return Result<Machine>.Failure(MachineryErrors.InvalidMachineData, MachineryError.InvalidMachineData);

        var code = command.Code.Trim();
        if (await machineRepository.ExistsByCodeAsync(code, cancellationToken))
            return Result<Machine>.Failure(MachineryErrors.DuplicateMachineCodeWithCode(code),
                MachineryError.DuplicateMachineCode);

        var machine = new Machine(command);
        await machineRepository.AddAsync(machine, cancellationToken);
        await unitOfWork.CompleteAsync(cancellationToken);
        return Result<Machine>.Success(machine);
    }

    public async Task<Result<Machine>> Handle(UpdateMachineStatusCommand command, CancellationToken cancellationToken)
    {
        if (!TryParseEnum<EMachineStatus>(command.Status, out var status))
            return Result<Machine>.Failure(MachineryErrors.InvalidMachineStatus, MachineryError.InvalidMachineStatus);

        var machine = await machineRepository.FindByIdAsync(command.MachineId, cancellationToken);
        if (machine is null)
            return Result<Machine>.Failure(MachineryErrors.MachineNotFound, MachineryError.MachineNotFound);

        machine.UpdateStatus(status);
        await unitOfWork.CompleteAsync(cancellationToken);
        return Result<Machine>.Success(machine);
    }

    public async Task<Result<MachineDowntime>> Handle(ReportMachineDowntimeCommand command,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(command.Reason) || command.StartTime is null)
            return Result<MachineDowntime>.Failure(MachineryErrors.InvalidDowntimeData,
                MachineryError.InvalidDowntimeData);

        var machine = await machineRepository.FindByIdAsync(command.MachineId, cancellationToken);
        if (machine is null)
            return Result<MachineDowntime>.Failure(MachineryErrors.MachineNotFound, MachineryError.MachineNotFound);

        if (machine.HasOpenDowntime)
            return Result<MachineDowntime>.Failure(MachineryErrors.MachineAlreadyStopped,
                MachineryError.MachineAlreadyStopped);

        var downtime = machine.ReportDowntime(command.Reason, command.StartTime.Value);
        await unitOfWork.CompleteAsync(cancellationToken);
        return Result<MachineDowntime>.Success(downtime);
    }

    public async Task<Result<MachineDowntime>> Handle(ResumeMachineOperationCommand command,
        CancellationToken cancellationToken)
    {
        if (command.EndTime is null)
            return Result<MachineDowntime>.Failure(MachineryErrors.InvalidDowntimeData,
                MachineryError.InvalidDowntimeData);

        var machine = await machineRepository.FindByIdAsync(command.MachineId, cancellationToken);
        if (machine is null)
            return Result<MachineDowntime>.Failure(MachineryErrors.MachineNotFound, MachineryError.MachineNotFound);

        var downtime = machine.Downtimes.FirstOrDefault(d => d.Id == command.DowntimeId);
        if (downtime is null)
            return Result<MachineDowntime>.Failure(MachineryErrors.DowntimeNotFound, MachineryError.DowntimeNotFound);

        if (!downtime.IsOpen)
            return Result<MachineDowntime>.Failure(MachineryErrors.DowntimeAlreadyClosed,
                MachineryError.DowntimeAlreadyClosed);

        if (command.EndTime.Value < downtime.StartTime)
            return Result<MachineDowntime>.Failure(MachineryErrors.InvalidDowntimePeriod,
                MachineryError.InvalidDowntimePeriod);

        machine.ResumeOperation(command.DowntimeId, command.EndTime.Value, command.Solution);
        await unitOfWork.CompleteAsync(cancellationToken);
        return Result<MachineDowntime>.Success(downtime);
    }

    public async Task<Result<MaintenanceRecord>> Handle(RegisterMaintenanceRecordCommand command,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(command.Technician) || command.PerformedDate is null)
            return Result<MaintenanceRecord>.Failure(MachineryErrors.InvalidMaintenanceData,
                MachineryError.InvalidMaintenanceData);

        if (!TryParseEnum<EMaintenanceType>(command.MaintenanceType, out var maintenanceType))
            return Result<MaintenanceRecord>.Failure(MachineryErrors.InvalidMaintenanceType,
                MachineryError.InvalidMaintenanceType);

        var machine = await machineRepository.FindByIdAsync(command.MachineId, cancellationToken);
        if (machine is null)
            return Result<MaintenanceRecord>.Failure(MachineryErrors.MachineNotFound, MachineryError.MachineNotFound);

        var maintenance = machine.RegisterMaintenance(command.Technician, maintenanceType, command.Description,
            command.PerformedDate.Value);
        await unitOfWork.CompleteAsync(cancellationToken);
        return Result<MaintenanceRecord>.Success(maintenance);
    }

    private static bool TryParseEnum<TEnum>(string? value, out TEnum result) where TEnum : struct, Enum
    {
        result = default;
        return !string.IsNullOrWhiteSpace(value) &&
               Enum.TryParse(value.Trim(), true, out result) &&
               Enum.IsDefined(result);
    }
}
