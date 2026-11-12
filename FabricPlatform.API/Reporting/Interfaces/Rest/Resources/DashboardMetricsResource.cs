namespace FabricPlatform.API.Reporting.Interfaces.Rest.Resources;

public record DashboardMetricsResource(string Date, int TotalProduced, int AveragePerHour, int DailyCompliance);
