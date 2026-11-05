using FabricPlatform.API.Alerts.Application.QueryServices;
using FabricPlatform.API.Alerts.Domain.Model.Aggregates;
using FabricPlatform.API.Alerts.Domain.Model.Queries;
using FabricPlatform.API.Alerts.Domain.Repositories;

namespace FabricPlatform.API.Alerts.Application.Internal.QueryServices;

public class AlertQueryService(
    IAlertRepository alertRepository) : IAlertQueryService
{
    public async Task<IEnumerable<Alert>> Handle(
        GetAllAlertsQuery query,
        CancellationToken cancellationToken)
    {
        return await alertRepository.ListAsync(cancellationToken);
    }
}