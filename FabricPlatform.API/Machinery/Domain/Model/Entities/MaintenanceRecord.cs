using FabricPlatform.API.Machinery.Domain.Model.ValueObjects;

namespace FabricPlatform.API.Machinery.Domain.Model.Entities;

public partial class MaintenanceRecord
{
    public MaintenanceRecord()
    {
        Technician = string.Empty;
        Description = string.Empty;
    }

    public MaintenanceRecord(MachineId machineId, string technician, EMaintenanceType maintenanceType,
        string description, DateTime performedDate) : this()
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(technician);
        MachineId = machineId;
        Technician = technician.Trim();
        MaintenanceType = maintenanceType;
        Description = description?.Trim() ?? string.Empty;
        PerformedDate = performedDate;
    }

    public MaintenanceRecordId Id { get; }
    public MachineId MachineId { get; private set; }
    public string Technician { get; private set; }
    public EMaintenanceType MaintenanceType { get; private set; }
    public string Description { get; private set; }
    public DateTime PerformedDate { get; private set; }
}
