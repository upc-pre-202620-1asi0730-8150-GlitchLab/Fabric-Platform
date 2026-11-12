using FabricPlatform.API.Batches.Domain.Model.Aggregates;
using FabricPlatform.API.Batches.Domain.Model.ValueObjects;
using FabricPlatform.API.Batches.Interfaces.Rest.Resources;

namespace FabricPlatform.API.Batches.Interfaces.Rest.Transform;

public static class BatchResourceFromEntityAssembler
{
    public static BatchResource ToResourceFromEntity(Batch entity)
    {
        return new BatchResource(
            Id: entity.Id.Value,
            BatchNumber: entity.Code.Value,
            GarmentModel: entity.GarmentModel,
            ProjectedQuantity: entity.ProjectedQuantity,
            ProcessedQuantity: entity.ProcessedQuantity,
            CurrentStage: entity.CurrentStage.Value,
            ProgressPercentage: entity.ProgressPercentage,
            DeliveryDate: entity.DeliveryDate,
            Status: entity.Status.ToDisplayName(),
            TechnicalSheetUrl: entity.TechnicalSheetUrl,
            Notes: entity.Notes
        );
    }
}