namespace FabricPlatform.API.Batches.Domain.Model.Commands;

public record CreateBatchObservationCommand(int BatchId, string Text);