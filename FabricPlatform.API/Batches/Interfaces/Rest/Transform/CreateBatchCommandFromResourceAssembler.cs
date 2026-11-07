using FabricPlatform.API.Batches.Domain.Model.Commands;
using FabricPlatform.API.Batches.Interfaces.Rest.Resources;

namespace FabricPlatform.API.Batches.Interfaces.Rest.Transform;

public static class CreateBatchCommandFromResourceAssembler
{
    public static CreateBatchCommand ToCommandFromResource(CreateBatchResource resource)
    {
        return new CreateBatchCommand
        {
            Code = resource.Code,
            ProjectedQuantity = resource.ProjectedQuantity,
            DeliveryDate = resource.DeliveryDate
        };
    }
}