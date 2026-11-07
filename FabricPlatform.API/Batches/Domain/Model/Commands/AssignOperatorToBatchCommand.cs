namespace FabricPlatform.API.Batches.Domain.Model.Commands;

public record AssignOperatorToBatchCommand
{
    public int BatchId { get; init; }
    public int OperatorId { get; init; }
    public string FullName { get; init; } = string.Empty;
    public string Specialty { get; init; } = string.Empty;
    public string Stage { get; init; } = string.Empty;
}