using FabricPlatform.API.Machinery.Domain.Model.ValueObjects;

namespace FabricPlatform.API.Machinery.Domain.Model.Commands;

public record ReportMachineDowntimeCommand(MachineId MachineId, string Reason, DateTime? StartTime)
{
    public ReportMachineDowntimeCommand(int machineId, string reason, DateTime? startTime)
        : this(new MachineId(machineId), reason, startTime)
    {
    }
}
