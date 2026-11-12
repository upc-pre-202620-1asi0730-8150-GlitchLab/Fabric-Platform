using FabricPlatform.API.Machinery.Domain.Model.ValueObjects;
using FabricPlatform.API.Shared.Domain.Model.Events;

namespace FabricPlatform.API.Machinery.Domain.Model.Events;

public record MachineDowntimeResumedEvent(MachineId MachineId, string Code, int AccumulatedDowntimeMinutes) : IEvent;
