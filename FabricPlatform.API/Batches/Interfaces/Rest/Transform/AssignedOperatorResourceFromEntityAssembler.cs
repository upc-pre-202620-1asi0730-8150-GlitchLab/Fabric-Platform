using FabricPlatform.API.Batches.Domain.Model.Entities;
using FabricPlatform.API.Batches.Interfaces.Rest.Resources;

namespace FabricPlatform.API.Batches.Interfaces.Rest.Transform;

public static class AssignedOperatorResourceFromEntityAssembler
{
    public static AssignedOperatorResource ToResourceFromEntity(BatchOperator assignedOperator)
    {
        return new AssignedOperatorResource(
            assignedOperator.Id,
            assignedOperator.BatchId.Value,
            assignedOperator.FullName,
            assignedOperator.Specialty,
            assignedOperator.AssignedStage.Value,
            assignedOperator.AssignmentDate);
    }
}
