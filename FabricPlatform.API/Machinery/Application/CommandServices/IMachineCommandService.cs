using FabricPlatform.API.Machinery.Domain.Model.Aggregates;
using FabricPlatform.API.Machinery.Domain.Model.Commands;
using FabricPlatform.API.Machinery.Domain.Model.Entities;
using FabricPlatform.API.Shared.Application.Model;

namespace FabricPlatform.API.Machinery.Application.CommandServices;

public interface IMachineCommandService
{
    Task<Result<Machine>> Handle(RegisterMachineCommand command, CancellationToken cancellationToken);

    Task<Result<Machine>> Handle(UpdateMachineStatusCommand command, CancellationToken cancellationToken);

    Task<Result<MachineDowntime>> Handle(ReportMachineDowntimeCommand command, CancellationToken cancellationToken);

    Task<Result<MachineDowntime>> Handle(ResumeMachineOperationCommand command, CancellationToken cancellationToken);

    Task<Result<MaintenanceRecord>> Handle(RegisterMaintenanceRecordCommand command,
        CancellationToken cancellationToken);
}
