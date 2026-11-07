namespace FabricPlatform.API.Batches.Domain.Model.Commands;

public record CreateBatchCommand
{
    public string Code { get; init; } = string.Empty;
    public int ProjectedQuantity { get; init; }
    public DateTimeOffset DeliveryDate { get; init; }
}