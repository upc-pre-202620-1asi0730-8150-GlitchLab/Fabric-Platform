using FabricPlatform.API.Machinery.Domain.Model.Events;
using FabricPlatform.API.Shared.Application.Internal.EventHandlers;
using Microsoft.Extensions.Logging;

namespace FabricPlatform.API.Machinery.Application.Internal.EventHandlers;

public class MachineRegisteredEventHandler(ILogger<MachineRegisteredEventHandler> logger)
    : IEventHandler<MachineRegisteredEvent>
{
    public Task Handle(MachineRegisteredEvent domainEvent, CancellationToken cancellationToken)
    {
        logger.LogInformation("Machine registered: {Code} (Type: {Type})", domainEvent.Code, domainEvent.Type);
        return Task.CompletedTask;
    }
}
