namespace FabricPlatform.API.Batches.Domain.Model.ValueObjects;

public static class BatchStatusExtensions
{
    public static bool TryParse(string? value, out BatchStatus status)
    {
        var normalized = value?.Replace(" ", string.Empty, StringComparison.Ordinal)
            .Replace("-", string.Empty, StringComparison.Ordinal)
            .Replace("_", string.Empty, StringComparison.Ordinal);

        return Enum.TryParse(normalized, true, out status) && Enum.IsDefined(status);
    }

    public static string ToDisplayName(this BatchStatus status) =>
        status switch
        {
            BatchStatus.InProduction => "In Production",
            BatchStatus.AtRisk => "At Risk",
            _ => status.ToString()
        };
}
