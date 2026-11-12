using FabricPlatform.API.Reporting.Domain.Model.ValueObjects;
using FabricPlatform.API.Reporting.Interfaces.Rest.Resources;

namespace FabricPlatform.API.Reporting.Interfaces.Rest.Transform;

public static class DashboardMetricsResourceFromEntityAssembler
{
    public static DashboardMetricsResource ToResourceFromEntity(DashboardMetrics entity)
    {
        return new DashboardMetricsResource(
            entity.Date.ToString("yyyy-MM-dd"),
            entity.TotalProduced,
            entity.AveragePerHour,
            entity.DailyCompliance);
    }
}
