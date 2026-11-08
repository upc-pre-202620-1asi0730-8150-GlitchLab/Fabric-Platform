using FabricPlatform.API.Machinery.Domain.Model.Entities;
using FabricPlatform.API.Machinery.Interfaces.Rest.Resources;

namespace FabricPlatform.API.Machinery.Interfaces.Rest.Transform;

public static class MachineDowntimeResourceFromEntityAssembler
{
    public static MachineDowntimeResource ToResourceFromEntity(MachineDowntime entity)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity),
                "MachineDowntime entity cannot be null when converting to resource.");

        return new MachineDowntimeResource(
            entity.Id.Value,
            entity.MachineId.Value,
            entity.Reason,
            entity.StartTime,
            entity.EndTime,
            entity.DurationMinutes,
            entity.Solution);
    }
}
