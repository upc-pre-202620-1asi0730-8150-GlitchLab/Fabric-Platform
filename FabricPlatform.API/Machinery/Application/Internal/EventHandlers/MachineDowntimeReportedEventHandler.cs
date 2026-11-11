using FabricPlatform.API.Machinery.Domain.Model.Events;
using FabricPlatform.API.Shared.Application.Internal.EventHandlers;
using Microsoft.Extensions.Logging;

namespace FabricPlatform.API.Machinery.Application.Internal.EventHandlers;

public class MachineDowntimeReportedEventHandler(ILogger<MachineDowntimeReportedEventHandler> logger)
    : IEventHandler<MachineDowntimeReportedEvent>
{
    public Task Handle(MachineDowntimeReportedEvent domainEvent, CancellationToken cancellationToken)
    {
        logger.LogWarning("Machine stopped: {Code} (ID: {MachineId}) - Reason: {Reason} at {StartTime}",
            domainEvent.Code, domainEvent.MachineId.Value, domainEvent.Reason, domainEvent.StartTime);
        return Task.CompletedTask;
    }
}
