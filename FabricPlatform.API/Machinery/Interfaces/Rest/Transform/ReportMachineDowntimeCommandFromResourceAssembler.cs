using FabricPlatform.API.Machinery.Domain.Model.Commands;
using FabricPlatform.API.Machinery.Interfaces.Rest.Resources;

namespace FabricPlatform.API.Machinery.Interfaces.Rest.Transform;

public static class ReportMachineDowntimeCommandFromResourceAssembler
{
    public static ReportMachineDowntimeCommand ToCommandFromResource(ReportMachineDowntimeResource resource, int machineId)
    {
        if (resource == null)
            throw new ArgumentNullException(nameof(resource),
                "ReportMachineDowntimeResource cannot be null when converting to command.");
        return new ReportMachineDowntimeCommand(machineId, resource.Reason, resource.StartTime);
    }
}
