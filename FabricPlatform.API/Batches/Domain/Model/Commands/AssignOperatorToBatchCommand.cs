namespace FabricPlatform.API.Batches.Domain.Model.Commands;

public record AssignOperatorToBatchCommand
{
    public int BatchId { get; init; }
    public string FullName { get; init; } = string.Empty;
}