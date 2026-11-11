using FabricPlatform.API.Batches.Domain.Model.Commands;
using FabricPlatform.API.Batches.Interfaces.Rest.Resources;

namespace FabricPlatform.API.Batches.Interfaces.Rest.Transform;

public static class CreateBatchObservationCommandFromResourceAssembler
{
    public static CreateBatchObservationCommand ToCommandFromResource(CreateObservationResource resource)
    {
        return new CreateBatchObservationCommand(
            resource.BatchId,
            resource.Text);
    }
}