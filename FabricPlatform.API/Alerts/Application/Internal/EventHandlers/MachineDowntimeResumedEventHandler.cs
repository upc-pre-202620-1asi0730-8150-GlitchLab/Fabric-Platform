using FabricPlatform.API.Alerts.Domain.Model.Aggregates;
using FabricPlatform.API.Alerts.Domain.Repositories;
using FabricPlatform.API.Machinery.Domain.Model.Events;
using FabricPlatform.API.Shared.Application.Internal.EventHandlers;

namespace FabricPlatform.API.Alerts.Application.Internal.EventHandlers;

public class MachineDowntimeResumedEventHandler(
    IAlertRepository alertRepository,
    IConfiguration configuration) : IEventHandler<MachineDowntimeResumedEvent>
{
    private const int DefaultLimitMinutes = 10;

    public async Task Handle(MachineDowntimeResumedEvent domainEvent, CancellationToken cancellationToken)
    {
        var limitMinutes = configuration.GetValue("Alerts:MachineDowntimeLimitMinutes", DefaultLimitMinutes);
        if (domainEvent.AccumulatedDowntimeMinutes <= limitMinutes) return;

        var machineId = domainEvent.MachineId.Value.ToString();
        var existing = await alertRepository.FindByTypeAndMachineAsync(Alert.MachineDowntimeType, machineId,
            cancellationToken);

        if (existing is null)
        {
            var alert = Alert.ForMachineDowntime(machineId, domainEvent.Code, domainEvent.AccumulatedDowntimeMinutes,
                limitMinutes);
            await alertRepository.AddAsync(alert, cancellationToken);
            return;
        }

        existing.RefreshMachineDowntime(domainEvent.AccumulatedDowntimeMinutes, limitMinutes);
        alertRepository.Update(existing);
    }
}
