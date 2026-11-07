namespace FabricPlatform.API.Batches.Interfaces.Rest.Resources;

public record UpdateBatchStageResource(
    string NewStage,
    int ProcessedQuantity
);