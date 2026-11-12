using FabricPlatform.API.Batches.Domain.Model.Entities;
using FabricPlatform.API.Batches.Domain.Repositories;
using FabricPlatform.API.Reporting.Application.QueryServices;
using FabricPlatform.API.Reporting.Domain.Model.Queries;
using FabricPlatform.API.Reporting.Domain.Model.ValueObjects;

namespace FabricPlatform.API.Reporting.Application.Internal.QueryServices;

public class DashboardMetricsQueryService(
    IBatchRepository batchRepository,
    IConfiguration configuration) : IDashboardMetricsQueryService
{
    private const int DefaultWorkingHoursPerDay = 8;

    public async Task<DashboardMetrics> Handle(GetDashboardMetricsByDateQuery query,
        CancellationToken cancellationToken)
    {
        var movements = await ListMovements(cancellationToken);
        var dayMovements = movements.Where(movement => ToDate(movement) == query.Date).ToList();
        return Summarize(query.Date, dayMovements);
    }

    public async Task<IEnumerable<DashboardMetrics>> Handle(GetDashboardMetricsHistoryQuery query,
        CancellationToken cancellationToken)
    {
        var movements = await ListMovements(cancellationToken);
        return movements
            .GroupBy(ToDate)
            .OrderBy(group => group.Key)
            .Select(group => Summarize(group.Key, group.ToList()))
            .ToList();
    }

    private async Task<List<BatchMovement>> ListMovements(CancellationToken cancellationToken)
    {
        var batches = await batchRepository.ListAsync(cancellationToken);
        return batches.SelectMany(batch => batch.Movements).ToList();
    }

    private static DateOnly ToDate(BatchMovement movement) => DateOnly.FromDateTime(movement.Timestamp.LocalDateTime);

    private DashboardMetrics Summarize(DateOnly date, IReadOnlyCollection<BatchMovement> movements)
    {
        var workingHours = configuration.GetValue("Reporting:WorkingHoursPerDay", DefaultWorkingHoursPerDay);
        var totalIn = movements.Sum(movement => movement.QtyIn);
        var totalOut = movements.Sum(movement => movement.QtyOut);
        var averagePerHour = workingHours > 0 ? (int)Math.Round((double)totalOut / workingHours) : 0;
        var compliance = totalIn > 0 ? (int)Math.Round(Math.Min(totalOut, totalIn) * 100.0 / totalIn) : 0;

        return new DashboardMetrics(date, totalOut, averagePerHour, compliance);
    }
}
