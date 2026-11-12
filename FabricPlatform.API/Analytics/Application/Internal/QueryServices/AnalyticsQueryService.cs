using FabricPlatform.API.Analytics.Application.Model;
using FabricPlatform.API.Analytics.Application.QueryServices;
using FabricPlatform.API.Analytics.Domain.Model.Queries;
using FabricPlatform.API.Batches.Domain.Repositories;

namespace FabricPlatform.API.Analytics.Application.Internal.QueryServices;

public class AnalyticsQueryService(
    IBatchRepository batchRepository)
    : IAnalyticsQueryService
{
    public async Task<DashboardMetricsResult> HandleAsync(
        GetDashboardMetricsQuery query,
        CancellationToken cancellationToken = default)
    {
        var batches = await batchRepository.ListAsync(cancellationToken);

        var startDate = query.Date.ToDateTime(TimeOnly.MinValue);
        var endDate = startDate.AddDays(1);

        var movements = batches
            .SelectMany(batch => batch.Movements)
            .Where(movement =>
                movement.Timestamp >= startDate &&
                movement.Timestamp < endDate)
            .ToList();

        var totalProduced = movements
            .Where(movement => movement.Difference > 0)
            .Sum(movement => movement.Difference);

        var hours = movements
            .Select(movement => movement.Timestamp.Hour)
            .Distinct()
            .Count();

        var averagePerHour = hours > 0
            ? (double)totalProduced / hours
            : 0;

        var projectedQuantity = batches
            .Where(batch =>
                batch.DeliveryDate.Date >= startDate.Date &&
                batch.DeliveryDate.Date <= endDate.Date)
            .Sum(batch => batch.ProjectedQuantity);

        var dailyCompliance = projectedQuantity > 0
            ? Math.Min(
                (double)totalProduced / projectedQuantity * 100,
                100)
            : 0;

        return new DashboardMetricsResult(
            query.Date,
            totalProduced,
            Math.Round(averagePerHour, 2),
            Math.Round(dailyCompliance, 2));
    }
}