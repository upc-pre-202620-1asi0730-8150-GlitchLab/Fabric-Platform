using FabricPlatform.API.Machinery.Domain.Model.Entities;
using FabricPlatform.API.Machinery.Interfaces.Rest.Resources;

namespace FabricPlatform.API.Machinery.Interfaces.Rest.Transform;

public static class MaintenanceRecordResourceFromEntityAssembler
{
    public static MaintenanceRecordResource ToResourceFromEntity(MaintenanceRecord entity)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity),
                "MaintenanceRecord entity cannot be null when converting to resource.");

        return new MaintenanceRecordResource(
            entity.Id.Value,
            entity.MachineId.Value,
            entity.Technician,
            entity.MaintenanceType.ToString(),
            entity.Description,
            entity.PerformedDate);
    }
}
