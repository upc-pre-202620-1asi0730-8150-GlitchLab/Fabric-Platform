using FabricPlatform.API.Shared.Domain.Model.Events;

namespace FabricPlatform.API.Machinery.Domain.Model.Events;

public record MachineRegisteredEvent(string Code, string Type) : IEvent;
