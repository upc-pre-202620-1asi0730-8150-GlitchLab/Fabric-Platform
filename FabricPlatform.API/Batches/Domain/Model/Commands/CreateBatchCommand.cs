namespace FabricPlatform.API.Batches.Domain.Model.Commands;

public record CreateBatchCommand
{
    string Code;
    int ProjectedQuantity;
    DateTimeOffset DeliveryDate;
}