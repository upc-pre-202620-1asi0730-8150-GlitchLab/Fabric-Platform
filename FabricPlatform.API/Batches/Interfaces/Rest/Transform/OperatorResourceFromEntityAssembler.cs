using FabricPlatform.API.Batches.Domain.Model.Aggregates;
using FabricPlatform.API.Batches.Interfaces.Rest.Resources;

namespace FabricPlatform.API.Batches.Interfaces.Rest.Transform;

public static class OperatorResourceFromEntityAssembler
{
    public static OperatorResource ToResourceFromEntity(ProductionOperator entity)
    {
        return new OperatorResource(entity.Id, entity.Name, entity.Specialty);
    }
}
