using FabricPlatform.API.Alerts.Domain.Model.Aggregates;
using FabricPlatform.API.Alerts.Interfaces.Rest.Resources;

namespace FabricPlatform.API.Alerts.Interfaces.Rest.Transform;

public static class AlertResourceFromEntityAssembler
{
    public static AlertResource ToResourceFromEntity(Alert entity)
    {
        return new AlertResource(
            entity.Id.Value.ToString(),
            entity.Status,
            entity.Type,
            entity.Reference,
            entity.Title,
            entity.Description,
            entity.Action,
            entity.Accumulated,
            entity.Limit,
            entity.Excess,
            entity.Machine,
            entity.Batch);
    }
}