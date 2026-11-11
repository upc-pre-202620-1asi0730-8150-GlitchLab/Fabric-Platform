using FabricPlatform.API.Machinery.Domain.Model.ValueObjects;

namespace FabricPlatform.API.Machinery.Domain.Model.Commands;

public record UpdateMachineStatusCommand(MachineId MachineId, string Status)
{
    public UpdateMachineStatusCommand(int machineId, string status) : this(new MachineId(machineId), status)
    {
    }
}
