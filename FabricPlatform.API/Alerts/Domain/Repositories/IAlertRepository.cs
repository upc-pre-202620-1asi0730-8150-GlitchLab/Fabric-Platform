using FabricPlatform.API.Alerts.Domain.Model.Aggregates;
using FabricPlatform.API.Shared.Domain.Repositories;

namespace FabricPlatform.API.Alerts.Domain.Repositories;

public interface IAlertRepository : IBaseRepository<Alert>
{
    Task<Alert?> FindByTypeAndMachineAsync(string type, string machineId, CancellationToken cancellationToken = default);
}