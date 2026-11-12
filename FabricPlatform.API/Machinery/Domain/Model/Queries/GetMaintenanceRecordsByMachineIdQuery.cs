using FabricPlatform.API.Machinery.Domain.Model.ValueObjects;

namespace FabricPlatform.API.Machinery.Domain.Model.Queries;

public record GetMaintenanceRecordsByMachineIdQuery(MachineId MachineId)
{
    public GetMaintenanceRecordsByMachineIdQuery(int machineId) : this(new MachineId(machineId))
    {
    }
}
