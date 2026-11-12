namespace FabricPlatform.API.Analytics.Interfaces.Rest.Resources;

public record DashboardMetricsResource(
    string Date,
    int TotalProduced,
    double AveragePerHour,
    double DailyCompliance
);