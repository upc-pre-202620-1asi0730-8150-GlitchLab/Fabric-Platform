using FabricPlatform.API.Machinery.Domain.Model.ValueObjects;
using FabricPlatform.API.Shared.Domain.Model.Events;

namespace FabricPlatform.API.Machinery.Domain.Model.Events;

public record MachineDowntimeReportedEvent(MachineId MachineId, string Code, string Reason, DateTime StartTime)
    : IEvent;
