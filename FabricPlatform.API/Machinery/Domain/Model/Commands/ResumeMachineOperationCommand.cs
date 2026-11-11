using FabricPlatform.API.Machinery.Domain.Model.ValueObjects;

namespace FabricPlatform.API.Machinery.Domain.Model.Commands;

public record ResumeMachineOperationCommand(
    MachineId MachineId,
    MachineDowntimeId DowntimeId,
    DateTime? EndTime,
    string? Solution)
{
    public ResumeMachineOperationCommand(int machineId, int downtimeId, DateTime? endTime, string? solution)
        : this(new MachineId(machineId), new MachineDowntimeId(downtimeId), endTime, solution)
    {
    }
}
