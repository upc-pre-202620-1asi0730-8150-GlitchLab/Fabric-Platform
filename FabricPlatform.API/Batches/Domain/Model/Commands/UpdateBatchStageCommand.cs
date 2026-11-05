namespace FabricPlatform.API.Batches.Domain.Model.Commands;

public class UpdateBatchStageCommand
{
    int BatchId { get; set; }
    string NewStage { get; set; }
    int ProcessedQuantity { get; set; }
}