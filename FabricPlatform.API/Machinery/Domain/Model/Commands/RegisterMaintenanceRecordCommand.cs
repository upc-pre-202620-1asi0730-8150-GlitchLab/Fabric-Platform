using FabricPlatform.API.Machinery.Domain.Model.ValueObjects;

namespace FabricPlatform.API.Machinery.Domain.Model.Commands;

public record RegisterMaintenanceRecordCommand(
    MachineId MachineId,
    string Technician,
    string MaintenanceType,
    string Description,
    DateTime? PerformedDate)
{
    public RegisterMaintenanceRecordCommand(int machineId, string technician, string maintenanceType,
        string description, DateTime? performedDate)
        : this(new MachineId(machineId), technician, maintenanceType, description, performedDate)
    {
    }
}
