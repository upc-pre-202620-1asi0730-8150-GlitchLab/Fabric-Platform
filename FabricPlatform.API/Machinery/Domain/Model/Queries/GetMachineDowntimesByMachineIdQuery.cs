using FabricPlatform.API.Machinery.Domain.Model.ValueObjects;

namespace FabricPlatform.API.Machinery.Domain.Model.Queries;

public record GetMachineDowntimesByMachineIdQuery(MachineId MachineId)
{
    public GetMachineDowntimesByMachineIdQuery(int machineId) : this(new MachineId(machineId))
    {
    }
}
