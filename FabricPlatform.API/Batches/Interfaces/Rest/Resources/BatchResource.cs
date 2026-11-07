namespace FabricPlatform.API.Batches.Interfaces.Rest.Resources;

public record BatchResource(
    int Id,
    string Code,
    string Status,
    int ProjectedQuantity,
    DateTimeOffset DeliveryDate
);