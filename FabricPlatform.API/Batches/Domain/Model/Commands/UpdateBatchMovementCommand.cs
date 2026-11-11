namespace FabricPlatform.API.Batches.Domain.Model.Commands;

public record UpdateBatchMovementCommand(
    int MovementId,
    string? Stage,
    DateTimeOffset? DateTime,
    int? QtyIn,
    int? QtyOut,
    string? Responsible,
    string? Status,
    int? Difference);
