using FabricPlatform.API.Batches.Domain.Model.Commands;
using FabricPlatform.API.Batches.Interfaces.Rest.Resources;

namespace FabricPlatform.API.Batches.Interfaces.Rest.Transform;

public static class CreateProductionOperatorCommandFromResourceAssembler
{
    public static CreateProductionOperatorCommand ToCommandFromResource(
        CreateProductionOperatorResource resource)
    {
        return new CreateProductionOperatorCommand(resource.Name, resource.Specialty);
    }
}
