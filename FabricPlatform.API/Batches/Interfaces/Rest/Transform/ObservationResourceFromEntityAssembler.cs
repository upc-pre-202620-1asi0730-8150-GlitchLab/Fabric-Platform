using FabricPlatform.API.Batches.Domain.Model.Entities;
using FabricPlatform.API.Batches.Interfaces.Rest.Resources;

namespace FabricPlatform.API.Batches.Interfaces.Rest.Transform;

public static class ObservationResourceFromEntityAssembler
{
    public static ObservationResource ToResourceFromEntity(BatchObservation observation)
    {
        return new ObservationResource(
            observation.Id,
            observation.BatchId.Value,
            observation.CreatedAt,
            observation.Content,
            observation.AuthorName);
    }
}
