namespace FabricPlatform.API.Reporting.Domain.Model.ValueObjects;

public record DashboardMetrics(DateOnly Date, int TotalProduced, int AveragePerHour, int DailyCompliance);
