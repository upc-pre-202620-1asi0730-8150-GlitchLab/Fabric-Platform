namespace FabricPlatform.API.Analytics.Application.Model;

public record DashboardMetricsResult(
    DateOnly Date,
    int TotalProduced,
    double AveragePerHour,
    double DailyCompliance
);