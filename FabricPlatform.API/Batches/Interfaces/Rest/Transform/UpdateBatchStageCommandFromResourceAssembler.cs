using FabricPlatform.API.Batches.Domain.Model.Commands;
using FabricPlatform.API.Batches.Interfaces.Rest.Resources;

namespace FabricPlatform.API.Batches.Interfaces.Rest.Transform;

public static class UpdateBatchStageCommandFromResourceAssembler
{
    public static UpdateBatchStageCommand ToCommandFromResource(int batchId, UpdateBatchStageResource resource)
    {
        return new UpdateBatchStageCommand
        {
            BatchId = batchId,
            NewStage = resource.NewStage,
            ProcessedQuantity = resource.ProcessedQuantity
        };
    }
}