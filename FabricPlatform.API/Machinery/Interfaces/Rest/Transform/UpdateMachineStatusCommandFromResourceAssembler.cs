using FabricPlatform.API.Machinery.Domain.Model.Commands;
using FabricPlatform.API.Machinery.Interfaces.Rest.Resources;

namespace FabricPlatform.API.Machinery.Interfaces.Rest.Transform;

public static class UpdateMachineStatusCommandFromResourceAssembler
{
    public static UpdateMachineStatusCommand ToCommandFromResource(UpdateMachineStatusResource resource, int machineId)
    {
        if (resource == null)
            throw new ArgumentNullException(nameof(resource),
                "UpdateMachineStatusResource cannot be null when converting to command.");
        return new UpdateMachineStatusCommand(machineId, resource.Status);
    }
}
