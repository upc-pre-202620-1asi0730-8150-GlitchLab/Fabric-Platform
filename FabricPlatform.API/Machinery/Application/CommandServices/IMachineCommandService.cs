using FabricPlatform.API.Machinery.Domain.Model.Aggregates;
using FabricPlatform.API.Machinery.Domain.Model.Commands;
using FabricPlatform.API.Machinery.Domain.Model.Entities;
using FabricPlatform.API.Shared.Application.Model;

namespace FabricPlatform.API.Machinery.Application.CommandServices;

/// <summary>
///     Represents the machine command service in the Fabric Platform.
/// </summary>
public interface IMachineCommandService
{
    /// <summary>
    ///     Handles the register machine command.
    /// </summary>
    /// <param name="command">The <see cref="RegisterMachineCommand" /> command to handle.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The registered <see cref="Machine" /> or the error.</returns>
    Task<Result<Machine>> Handle(RegisterMachineCommand command, CancellationToken cancellationToken);

    /// <summary>
    ///     Handles the update machine status command.
    /// </summary>
    /// <param name="command">The <see cref="UpdateMachineStatusCommand" /> command to handle.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The updated <see cref="Machine" /> or the error.</returns>
    Task<Result<Machine>> Handle(UpdateMachineStatusCommand command, CancellationToken cancellationToken);

    /// <summary>
    ///     Handles the report machine downtime command.
    /// </summary>
    /// <param name="command">The <see cref="ReportMachineDowntimeCommand" /> command to handle.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The registered <see cref="MachineDowntime" /> or the error.</returns>
    Task<Result<MachineDowntime>> Handle(ReportMachineDowntimeCommand command, CancellationToken cancellationToken);

    /// <summary>
    ///     Handles the resume machine operation command.
    /// </summary>
    /// <param name="command">The <see cref="ResumeMachineOperationCommand" /> command to handle.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The closed <see cref="MachineDowntime" /> or the error.</returns>
    Task<Result<MachineDowntime>> Handle(ResumeMachineOperationCommand command, CancellationToken cancellationToken);

    /// <summary>
    ///     Handles the register maintenance record command.
    /// </summary>
    /// <param name="command">The <see cref="RegisterMaintenanceRecordCommand" /> command to handle.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The registered <see cref="MaintenanceRecord" /> or the error.</returns>
    Task<Result<MaintenanceRecord>> Handle(RegisterMaintenanceRecordCommand command,
        CancellationToken cancellationToken);
}
