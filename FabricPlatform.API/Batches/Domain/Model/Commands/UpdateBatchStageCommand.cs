namespace FabricPlatform.API.Batches.Domain.Model.Commands;

public record UpdateBatchStageCommand
{
    public int BatchId { get; set; }
    public string NewStage { get; set; } = string.Empty;
    public int ProcessedQuantity { get; set; }
}