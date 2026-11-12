namespace FabricPlatform.API.Batches.Interfaces.Rest.Resources;

public record AssignedOperatorResource(
    int Id,
    int BatchId,
    string OperatorName,
    string Specialty,
    string Stage,
    DateTimeOffset AssignedAt);
