using FabricPlatform.API.Batches.Domain.Model.ValueObjects;

namespace FabricPlatform.API.Batches.Domain.Model;

public record BatchTraceabilitySummary(
    int BatchId,
    int ProjectedQuantity,
    int CurrentQuantity,
    int Difference,
    StageName CurrentStage);