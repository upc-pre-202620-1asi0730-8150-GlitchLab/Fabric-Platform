namespace FabricPlatform.API.Machinery.Interfaces.Rest.Resources;

public record MaintenanceRecordResource(
    int Id,
    int MachineId,
    string Technician,
    string MaintenanceType,
    string Description,
    DateTime PerformedDate);
