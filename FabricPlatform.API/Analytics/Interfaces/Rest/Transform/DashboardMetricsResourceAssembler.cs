using FabricPlatform.API.Analytics.Application.Model;
using FabricPlatform.API.Analytics.Interfaces.Rest.Resources;

namespace FabricPlatform.API.Analytics.Interfaces.Rest.Transform;

public static class DashboardMetricsResourceAssembler
{
    public static DashboardMetricsResource ToResourceFromResult(
        DashboardMetricsResult result)
    {
        return new DashboardMetricsResource(
            result.Date.ToString("yyyy-MM-dd"),
            result.TotalProduced,
            result.AveragePerHour,
            result.DailyCompliance
        );
    }
}