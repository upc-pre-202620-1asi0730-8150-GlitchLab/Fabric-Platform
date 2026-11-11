using FabricPlatform.API.Batches.Domain.Model.Entities;
using FabricPlatform.API.Batches.Interfaces.Rest.Resources;

namespace FabricPlatform.API.Batches.Interfaces.Rest.Transform;

public static class TraceabilityMovementResourceFromEntityAssembler
{
    public static TraceabilityMovementResource ToResourceFromEntity(BatchMovement movement)
    {
        return new TraceabilityMovementResource(
            movement.Id,
            movement.BatchId.Value,
            movement.StageName.Value,
            movement.Timestamp,
            movement.QtyIn,
            movement.QtyOut,
            movement.Difference,
            movement.ResponsiblePerson,
            movement.Status);
    }
}
