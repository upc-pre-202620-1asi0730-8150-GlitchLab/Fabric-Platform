namespace FabricPlatform.API.Batches.Interfaces.Rest.Resources;

public record CreateBatchResource(
    string Code,
    int ProjectedQuantity,
    DateTimeOffset DeliveryDate
);