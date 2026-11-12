namespace FabricPlatform.API.Batches.Interfaces.Rest.Resources;

public record TraceabilitySummaryResource(
    int BatchId,
    int ProjectedQuantity,
    int CurrentQuantity,
    int Difference,
    string CurrentStage);