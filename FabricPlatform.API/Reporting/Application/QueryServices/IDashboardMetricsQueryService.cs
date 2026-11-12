using FabricPlatform.API.Reporting.Domain.Model.Queries;
using FabricPlatform.API.Reporting.Domain.Model.ValueObjects;

namespace FabricPlatform.API.Reporting.Application.QueryServices;

public interface IDashboardMetricsQueryService
{
    Task<DashboardMetrics> Handle(GetDashboardMetricsByDateQuery query, CancellationToken cancellationToken);

    Task<IEnumerable<DashboardMetrics>> Handle(GetDashboardMetricsHistoryQuery query,
        CancellationToken cancellationToken);
}
