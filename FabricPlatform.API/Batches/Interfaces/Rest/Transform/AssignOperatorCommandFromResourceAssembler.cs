using FabricPlatform.API.Batches.Domain.Model.Commands;
using FabricPlatform.API.Batches.Interfaces.Rest.Resources;

namespace FabricPlatform.API.Batches.Interfaces.Rest.Transform;

public static class AssignOperatorCommandFromResourceAssembler
{
    public static AssignOperatorToBatchCommand ToCommandFromResource(int batchId, AssignOperatorResource resource)
    {
        return new AssignOperatorToBatchCommand
        {
            BatchId = resource.BatchId > 0 ? resource.BatchId : batchId,
            FullName = resource.OperatorName
        };
    }
}