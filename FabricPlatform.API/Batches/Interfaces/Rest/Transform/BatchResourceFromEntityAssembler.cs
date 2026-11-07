using FabricPlatform.API.Batches.Domain.Model.Aggregates;
using FabricPlatform.API.Batches.Interfaces.Rest.Resources;

namespace FabricPlatform.API.Batches.Interfaces.Rest.Transform;

public static class BatchResourceFromEntityAssembler
{
    public static BatchResource ToResourceFromEntity(Batch entity)
    {
        return new BatchResource(
            Id: entity.Id.Value,
            Code: entity.Code.Value,
            Status: entity.Status.ToString(),
            ProjectedQuantity: entity.ProjectedQuantity,
            DeliveryDate: entity.DeliveryDate
        );
    }
}