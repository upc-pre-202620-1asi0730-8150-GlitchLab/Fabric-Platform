namespace FabricPlatform.API.Batches.Interfaces.Rest.Resources;

public record TraceabilityMovementResource(
    int Id,
    int BatchId,
    string Stage,
    DateTimeOffset DateTime,
    int QtyIn,
    int QtyOut,
    int Difference,
    string Responsible,
    string Status);
