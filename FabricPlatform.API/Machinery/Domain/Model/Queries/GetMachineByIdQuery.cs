using FabricPlatform.API.Machinery.Domain.Model.ValueObjects;

namespace FabricPlatform.API.Machinery.Domain.Model.Queries;

public record GetMachineByIdQuery(MachineId MachineId)
{
    public GetMachineByIdQuery(int machineId) : this(new MachineId(machineId))
    {
    }
}
