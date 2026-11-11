using FabricPlatform.API.Machinery.Domain.Model.Commands;
using FabricPlatform.API.Machinery.Interfaces.Rest.Resources;

namespace FabricPlatform.API.Machinery.Interfaces.Rest.Transform;

public static class RegisterMaintenanceRecordCommandFromResourceAssembler
{
    public static RegisterMaintenanceRecordCommand ToCommandFromResource(RegisterMaintenanceRecordResource resource, int machineId)
    {
        if (resource == null)
            throw new ArgumentNullException(nameof(resource),
                "RegisterMaintenanceRecordResource cannot be null when converting to command.");
        return new RegisterMaintenanceRecordCommand(machineId, resource.Technician, resource.MaintenanceType, resource.Description, resource.PerformedDate);
    }
}
