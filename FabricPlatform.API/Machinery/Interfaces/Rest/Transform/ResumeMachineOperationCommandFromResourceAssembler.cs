using FabricPlatform.API.Machinery.Domain.Model.Commands;
using FabricPlatform.API.Machinery.Interfaces.Rest.Resources;

namespace FabricPlatform.API.Machinery.Interfaces.Rest.Transform;

public static class ResumeMachineOperationCommandFromResourceAssembler
{
    public static ResumeMachineOperationCommand ToCommandFromResource(ResumeMachineOperationResource resource, int machineId, int downtimeId)
    {
        if (resource == null)
            throw new ArgumentNullException(nameof(resource),
                "ResumeMachineOperationResource cannot be null when converting to command.");
        return new ResumeMachineOperationCommand(machineId, downtimeId, resource.EndTime, resource.Solution);
    }
}
