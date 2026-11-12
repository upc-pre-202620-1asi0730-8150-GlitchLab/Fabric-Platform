using FabricPlatform.API.Analytics.Application.Model;
using FabricPlatform.API.Analytics.Domain.Model.Queries;

namespace FabricPlatform.API.Analytics.Application.QueryServices;

public interface IAnalyticsQueryService
{
    Task<DashboardMetricsResult> HandleAsync(
        GetDashboardMetricsQuery query,
        CancellationToken cancellationToken = default);
}