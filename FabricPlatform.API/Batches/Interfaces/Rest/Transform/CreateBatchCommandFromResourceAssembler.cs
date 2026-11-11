using FabricPlatform.API.Batches.Domain.Model.Commands;
using FabricPlatform.API.Batches.Interfaces.Rest.Resources;

namespace FabricPlatform.API.Batches.Interfaces.Rest.Transform;

public static class CreateBatchCommandFromResourceAssembler
{
    public static CreateBatchCommand ToCommandFromResource(CreateBatchResource resource)
    {
        return new CreateBatchCommand(
            resource.GarmentModel ?? string.Empty,
            resource.ProjectedQuantity ?? 0,
            resource.DeliveryDate ?? default,
            resource.Notes);
    }
}