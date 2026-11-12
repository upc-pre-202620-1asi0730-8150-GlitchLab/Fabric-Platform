using FabricPlatform.API.Machinery.Domain.Model.Aggregates;
using FabricPlatform.API.Machinery.Interfaces.Rest.Resources;

namespace FabricPlatform.API.Machinery.Interfaces.Rest.Transform;

public static class MachineResourceFromEntityAssembler
{
    public static MachineResource ToResourceFromEntity(Machine entity)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity),
                "Machine entity cannot be null when converting to resource.");

        return new MachineResource(
            entity.Id.Value,
            entity.Code,
            entity.Type,
            entity.Status.ToString(),
            entity.Location,
            entity.AcquiredDate,
            entity.AccumulatedDowntimeMinutes);
    }
}
