using FabricPlatform.API.Machinery.Domain.Model.Commands;
using FabricPlatform.API.Machinery.Domain.Model.Entities;
using FabricPlatform.API.Machinery.Domain.Model.Events;
using FabricPlatform.API.Machinery.Domain.Model.ValueObjects;
using FabricPlatform.API.Shared.Domain.Model.Aggregates.Internal;

namespace FabricPlatform.API.Machinery.Domain.Model.Aggregates;

public partial class Machine : AggregateRoot
{
    public Machine()
    {
        Code = string.Empty;
        Type = string.Empty;
        Location = string.Empty;
        Status = EMachineStatus.Operational;
        Downtimes = new List<MachineDowntime>();
        Maintenances = new List<MaintenanceRecord>();
    }

    public Machine(string code, string type, string location, DateTime acquiredDate) : this()
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(code);
        ArgumentException.ThrowIfNullOrWhiteSpace(type);
        ArgumentException.ThrowIfNullOrWhiteSpace(location);
        Code = code.Trim();
        Type = type.Trim();
        Location = location.Trim();
        AcquiredDate = acquiredDate;
        AddDomainEvent(new MachineRegisteredEvent(Code, Type));
    }

    public Machine(RegisterMachineCommand command)
        : this(command.Code, command.Type, command.Location, command.AcquiredDate!.Value)
    {
    }

    public MachineId Id { get; }

    public string Code { get; private set; }

    public string Type { get; private set; }

    public EMachineStatus Status { get; private set; }

    public string Location { get; private set; }

    public DateTime AcquiredDate { get; private set; }

    public ICollection<MachineDowntime> Downtimes { get; }

    public ICollection<MaintenanceRecord> Maintenances { get; }

    public int AccumulatedDowntimeMinutes => Downtimes.Sum(downtime => downtime.DurationMinutes ?? 0);

    public bool HasOpenDowntime => Downtimes.Any(downtime => downtime.IsOpen);

    public void UpdateStatus(EMachineStatus status)
    {
        Status = status;
    }

    public MachineDowntime ReportDowntime(string reason, DateTime startTime)
    {
        if (HasOpenDowntime)
            throw new InvalidOperationException("The machine already has an open downtime.");

        var downtime = new MachineDowntime(Id, reason, startTime);
        Downtimes.Add(downtime);
        Status = EMachineStatus.Stopped;
        AddDomainEvent(new MachineDowntimeReportedEvent(Id, Code, downtime.Reason, startTime));
        return downtime;
    }

    public MachineDowntime ResumeOperation(MachineDowntimeId downtimeId, DateTime endTime, string? solution)
    {
        var downtime = Downtimes.FirstOrDefault(d => d.Id == downtimeId)
                       ?? throw new InvalidOperationException("The downtime does not belong to this machine.");
        downtime.Close(endTime, solution);
        Status = EMachineStatus.Operational;
        AddDomainEvent(new MachineDowntimeResumedEvent(Id, Code, AccumulatedDowntimeMinutes));
        return downtime;
    }

    public MaintenanceRecord RegisterMaintenance(string technician, EMaintenanceType maintenanceType,
        string description, DateTime performedDate)
    {
        var maintenance = new MaintenanceRecord(Id, technician, maintenanceType, description, performedDate);
        Maintenances.Add(maintenance);
        return maintenance;
    }
}
