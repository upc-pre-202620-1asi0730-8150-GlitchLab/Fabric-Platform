using FabricPlatform.API.Alerts.Domain.Model.Aggregates;
using FabricPlatform.API.Alerts.Domain.Model.Queries;

namespace FabricPlatform.API.Alerts.Application.QueryServices;

public interface IAlertQueryService
{
    Task<IEnumerable<Alert>> Handle(
        GetAllAlertsQuery query,
        CancellationToken cancellationToken);
}